using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform jugador;
    
    [Header("Distancia de la cámara")]
    // X=0 (centrado), Y=3 (altura), Z=-6 (atrás del jugador)
    public Vector3 compensacion = new Vector3(0, 3f, -6f); 

    void LateUpdate()
    {
        if (jugador != null)
        {
            // Mantiene la cámara siempre a la misma distancia del jugador
            transform.position = jugador.position + compensacion;
        }
    }
}