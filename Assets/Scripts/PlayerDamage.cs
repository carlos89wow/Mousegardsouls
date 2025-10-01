using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Invulnerabilidad")]
    public float invulTime = 0.35f;    // tiempo invulnerable tras recibir daño
    private bool invulnerable = false;

    [Header("Feedback")]
    public SpriteRenderer sr;          // se asigna solo si es null
    public float blinkInterval = 0.06f;

    [Header("Knockback")]
    public float knockForce = 4f;      // fuerza del empujón
    private Rigidbody2D rb;

    // para daño periódico (tick)
    private float lastTickTime = -999f;

    void Awake()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    /// <summary>Daño instantáneo (usa invulnerabilidad y feedback)</summary>
    public void TakeDamage(int amount, Vector2? fromWorldPos = null)
    {
        if (invulnerable) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        // Knockback opcional
        if (fromWorldPos.HasValue && rb != null)
        {
            Vector2 dir = ((Vector2)transform.position - fromWorldPos.Value).normalized;
            rb.AddForce(dir * knockForce, ForceMode2D.Impulse);
        }

        // feedback de daño + invulnerabilidad breve
        StartCoroutine(DamageFeedback());

        if (currentHealth <= 0)
        {
            // TODO: muerte (desactivar input, animación, etc.)
            // for now:
            Debug.Log("Player muerto");
        }
    }

    /// <summary>Daño periódico: solo aplica si transcurrió 'tick' desde el último daño</summary>
    public void TryTickDamage(int amount, float tick, Vector2? fromWorldPos = null)
    {
        if (Time.time - lastTickTime < tick) return;
        lastTickTime = Time.time;
        TakeDamage(amount, fromWorldPos);
    }

    private IEnumerator DamageFeedback()
    {
        invulnerable = true;

        float end = Time.time + invulTime;
        bool visible = true;

        while (Time.time < end)
        {
            visible = !visible;
            if (sr != null) sr.enabled = visible;
            yield return new WaitForSeconds(blinkInterval);
        }

        if (sr != null) sr.enabled = true;
        invulnerable = false;
    }
}
