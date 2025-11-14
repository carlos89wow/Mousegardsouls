using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public bool useKeyboard = true;

    [Header("Knockback / Invulnerabilidad")]
    public float knockbackDistance = 1f;
    public float knockbackDuration = 0.08f;
    public float invulnTime = 0.4f;

    // Componentes
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // Estado
    private Vector2 input = Vector2.zero;
    private bool isKnockback = false;
    private bool isInvulnerable = false;

    [Header("Armas y disparo")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    [SerializeField] private float shootCooldown = 0.3f;
    private float shootTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        input = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        shootTimer = 0f; // Reinicia el temporizador
    }

    void Update()
    {
        // Movimiento por teclado (solo si se usa)
        if (useKeyboard && !isKnockback)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input = input.normalized;
        }

        // Enfriamiento de disparo
        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;

        // Disparo con espacio (opcional)
        if (useKeyboard && Input.GetKeyDown(KeyCode.Space))
            PressShoot();

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (isKnockback) return;
        rb.linearVelocity = input * speed;
    }

    // ===== Botones UI =====
    public void MoveUp() { if (!isKnockback) input = Vector2.up; }
    public void MoveDown() { if (!isKnockback) input = Vector2.down; }
    public void MoveLeft() { if (!isKnockback) { input = Vector2.left; if (sr) sr.flipX = true; } }
    public void MoveRight() { if (!isKnockback) { input = Vector2.right; if (sr) sr.flipX = false; } }
    public void Stop() { if (!isKnockback) input = Vector2.zero; }

    // ===== Colisiones =====
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isInvulnerable) return;
        if (col.collider.CompareTag("Enemy"))
        {
            LevelManager.Instance.RestartLevel();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvulnerable) return;
        if (other.CompareTag("Enemy") || other.CompareTag("Hazard"))
        {
            LevelManager.Instance.RestartLevel();
        }

    }

    private IEnumerator DoKnockback(Vector2 fromPosition)
    {
        isKnockback = true;
        isInvulnerable = true;

        if (anim) anim.SetFloat("Speed", 0f);

        Vector2 dir = ((Vector2)transform.position - fromPosition).normalized;
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.up;

        Vector2 start = rb.position;
        Vector2 target = start + dir * knockbackDistance;

        rb.linearVelocity = Vector2.zero;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime / knockbackDuration;
            rb.MovePosition(Vector2.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t)));
            yield return new WaitForFixedUpdate();
        }

        isKnockback = false;

        float timer = 0f;
        while (timer < invulnTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        isInvulnerable = false;
    }

    private void UpdateAnimation()
    {
        if (!anim) return;

        float currentSpeed = rb.linearVelocity.magnitude;
        anim.SetFloat("Speed", currentSpeed);
        //anim.SetFloat("Horizontal", input.x);
       // anim.SetFloat("Vertical", input.y);
    }

    // ===== Disparo =====
    private void ShootArrow()
    {
        if (arrowPrefab == null) return;

        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, transform.rotation);

        Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
        if (rbArrow != null)
        {
            rbArrow.linearVelocity = transform.right * 10f;
        }

        Debug.Log("Flecha disparada");
    }

    public void PressShoot()
    {
        if (shootTimer > 0f)
        {
            Debug.Log("Cooldown activo");
            return;
        }

        ShootArrow();
        shootTimer = shootCooldown;
    }
    public void Die()
    {
        Debug.Log("El jugador murió");
        LevelManager.Instance.RestartLevel();
    }

}
