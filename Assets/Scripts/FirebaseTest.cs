using UnityEngine;
using Firebase;
using Firebase.Firestore;
using System.Threading.Tasks;

public class FirebaseTest : MonoBehaviour
{
    async void Start()
    {
        // Verifica dependencias de Firebase
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("✅ Firebase inicializado correctamente");

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

            // Datos de prueba
            var data = new
            {
                mensaje = "Conexión exitosa",
                fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await db.Collection("pruebas").Document("conexion").SetAsync(data);
            Debug.Log("✅ Documento guardado correctamente en Firestore");
        }
        else
        {
            Debug.LogError("❌ Error al inicializar Firebase: " + dependencyStatus);
        }
    }
}
