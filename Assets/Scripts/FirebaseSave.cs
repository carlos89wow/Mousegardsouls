using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;

public class FirebaseSave : MonoBehaviour
{
    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        Debug.Log("✅ Firestore listo para guardar y leer datos");
    }

    // 🔹 Método para guardar datos (con valores de ejemplo)
    public void GuardarJugador()
    {
        Dictionary<string, object> datos = new Dictionary<string, object>
        {
            { "nombre", "Brienne" },
            { "nivel", 3 },
            { "flechas", 12 },
            { "monedas", 150 }
        };

        db.Collection("jugadores").Document("Brienne").SetAsync(datos).ContinueWith(task =>
        {
            if (task.IsCompleted)
                Debug.Log("💾 Datos guardados de Brienne");
            else
                Debug.LogError("❌ Error al guardar: " + task.Exception);
        });
    }

    // 🔹 Método para leer los datos
    public void LeerJugador()
    {
        db.Collection("jugadores").Document("Brienne").GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.Result.Exists)
            {
                var doc = task.Result;
                Debug.Log($"📖 Jugador: {doc.Id}");
                Debug.Log($"Nivel: {doc.GetValue<int>("nivel")}, Flechas: {doc.GetValue<int>("flechas")}, Monedas: {doc.GetValue<int>("monedas")}");
            }
            else
            {
                Debug.LogWarning("⚠ No se encontró el jugador Brienne");
            }
        });
    }
}
