using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    public int arrowAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerArrows playerArrows = other.GetComponent<PlayerArrows>();
            if (playerArrows != null)
            {
                playerArrows.AddArrows(arrowAmount);
                Destroy(gameObject);
            }
        }
    }
}
