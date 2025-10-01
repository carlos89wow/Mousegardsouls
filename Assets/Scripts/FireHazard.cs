using UnityEngine;

public class FireHazard : MonoBehaviour
{
    [Header("Da�o")]
    public int damage = 1;
    public float tick = 0.5f; // da�o peri�dico cada X s, si te interesa

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var pd = other.GetComponent<PlayerDamage>();
        if (pd != null)
        {
            // Da�o instant�neo + knockback desde ESTA hoguera
            pd.TakeDamage(damage, transform.position);
        }
    }

    // (Opcional) da�o peri�dico si el player se queda encima
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
