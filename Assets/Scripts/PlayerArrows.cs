using UnityEngine;

public class PlayerArrows : MonoBehaviour
{
    public int maxArrows = 10;
    public int currentArrows = 0;

    public void AddArrows(int amount)
    {
        currentArrows = Mathf.Min(currentArrows + amount, maxArrows);
        Debug.Log("Flechas: " + currentArrows);
    }

    public void UseArrow()
    {
        if (currentArrows > 0)
        {
            currentArrows--;
            Debug.Log("Disparó una flecha. Restan: " + currentArrows);
            // Aquí llamas al método que instancia la flecha en el juego
        }
        else
        {
            Debug.Log("No tienes flechas");
        }
    }
}
