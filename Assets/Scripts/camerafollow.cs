using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Asigna aquí el objeto del jugador
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Ajusta para centrar al jugador correctamente

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
