using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 1;      // Nivel actual
    public float difficulty = 1f;     // Factor de dificultad
    public float difficultyStep = 0.3f; // Aumento por nivel

    public static LevelManager Instance;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // =====================================
    // 🔵 Cuando el jugador completa el nivel
    // =====================================
    public void LevelCompleted()
    {
        currentLevel++;
        difficulty += difficultyStep;

        Debug.Log("Nivel completado. Nuevo nivel: " + currentLevel);
        Debug.Log("Dificultad actual: " + difficulty);

        // Ejemplo: aumentar la velocidad del jugador
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.speed = 5f + difficulty;

        // Aquí podrías cargar la siguiente escena
        // SceneManager.LoadScene("Nivel2");
    }

    // =====================================
    // 🔴 Reiniciar nivel cuando mueres
    // =====================================
    public void RestartLevel()
    {
        Debug.Log("Reiniciando nivel...");

        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
