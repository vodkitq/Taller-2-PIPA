using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float velocidadMuerte = 3f;

    void Update()
    {
        // El zombie se mueve constantemente hacia atrás (Z negativo)
        // Como el jugador viene hacia adelante, se encontrarán de frente
        transform.Translate(Vector3.back * velocidadMuerte * Time.deltaTime);
    }
}