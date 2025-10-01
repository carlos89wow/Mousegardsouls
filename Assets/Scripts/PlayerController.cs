using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public bool useKeyboard = true;      // Si usas botones UI, déjalo en false

    [Header("Knockback / Invulnerabilidad")]
    public float knockbackDistance = 1f; // Distancia a “rebotar”
    public float knockbackDuration = 0.08f;
    public float invulnTime = 0.4f;      // Tiempo invulnerable tras el golpe

    // Componentes
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // Estado de entrada y flags
    private Vector2 input = Vector2.zero;
    private bool isKnockback = false;
    private bool isInvulnerable = false;

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
    }

    void Update()
    {
        // Entrada por teclado (opcional)
        if (useKeyboard && !isKnockback)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input = input.normalized;
        }

        // Actualiza animaciones cada frame
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (isKnockback) return;
        rb.linearVelocity = input * speed;
    }

    // ===== Botones UI (úsalos si no quieres teclado) =====
    public void MoveUp() { if (!isKnockback) input = Vector2.up; }
    public void MoveDown() { if (!isKnockback) input = Vector2.down; }
    public void MoveLeft() { if (!isKnockback) { input = Vector2.left; if (sr) sr.flipX = true; } }
    public void MoveRight() { if (!isKnockback) { input = Vector2.right; if (sr) sr.flipX = false; } }
    public void Stop() { if (!isKnockback) input = Vector2.zero; }

    // ===== Colisiones (elige trigger o collision según tu setup) =====
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isInvulnerable) return;
        if (col.collider.CompareTag("Enemy"))
        {
            Vector2 from = col.collider.bounds.center;
            StartCoroutine(DoKnockback(from));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvulnerable) return;
        if (other.CompareTag("Enemy") || other.CompareTag("Hazard"))
        {
            Vector2 from = other.bounds.center;
            StartCoroutine(DoKnockback(from));
        }
    }

    private IEnumerator DoKnockback(Vector2 fromPosition)
    {
        isKnockback = true;
        isInvulnerable = true;

        // “Corta” la animación de caminar
        if (anim) anim.SetFloat("Speed", 0f);

        // Dirección del empujón (del enemigo hacia el jugador)
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

        // Invulnerabilidad temporal (si quieres, aquí puedes parpadear el sprite)
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

        // Velocidad actual para el parámetro "Speed"
        float currentSpeed = rb.linearVelocity.magnitude;
        anim.SetFloat("Speed", currentSpeed);

        // Si usas blend por dirección (opcional)
        anim.SetFloat("Horizontal", input.x);
        anim.SetFloat("Vertical", input.y);
    }
}
