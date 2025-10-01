using UnityEngine;

public class FireHazard : MonoBehaviour
{
    [Header("Daño")]
    public int damage = 1;
    public float tick = 0.5f; // daño periódico cada X s, si te interesa

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var pd = other.GetComponent<PlayerDamage>();
        if (pd != null)
        {
            // Daño instantáneo + knockback desde ESTA hoguera
            pd.TakeDamage(damage, transform.position);
        }
    }

    // (Opcional) daño periódico si el player se queda encima
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var pd = other.GetComponent<PlayerDamage>();
        if (pd != null)
        {
            pd.TryTickDamage(damage, tick);
        }
    }
}
