using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 input = Vector2.zero;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;   // ← NUEVO: para voltear el sprite

    public bool useKeyboard = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();  // ← toma el del mismo GameObject
    }

    void OnEnable()
    {
        input = Vector2.zero;
        if (rb != null) rb.linearVelocity = Vector2.zero;   // usa rb.velocity si lo prefieres
    }

    void Start()
    {
        input = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (useKeyboard)
        {
            input = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

            // ← NUEVO: si usas teclado, voltea según la dirección X
            if (input.x != 0f) sr.flipX = (input.x < 0f);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * speed;

        // Animación Idle/Walk
        anim.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);
    }

    // ===== Botones UI =====
    public void MoveUp() { input = Vector2.up; }
    public void MoveDown() { input = Vector2.down; }
    public void MoveLeft() { input = Vector2.left; sr.flipX = true; }  // ← NUEVO
    public void MoveRight() { input = Vector2.right; sr.flipX = false; }  // ← NUEVO
    public void Stop() { input = Vector2.zero; }
}
