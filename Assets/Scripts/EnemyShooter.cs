using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float shootRate = 1.2f;
    public float arrowSpeed = 8f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(ShootAtPlayer), 1f, shootRate);
    }

    void ShootAtPlayer()
    {
        if (!player || !arrowPrefab) return;

        Vector2 dir = (player.position - transform.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab,
            shootPoint ? shootPoint.position : transform.position,
            Quaternion.identity);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = dir * arrowSpeed;
    }
}
