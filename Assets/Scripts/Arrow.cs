using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Puedes poner aquí condiciones según el tipo de objeto
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            // Si toca enemigo o pared, destruye la flecha
            Destroy(gameObject);
        }
    }


}
