using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script adjunto al Prefab de la Moneda.
/// Detecta la colisión (Trigger) con el jugador para sumar puntaje y rotar sobre sí misma.
/// </summary>
public class Moneda : MonoBehaviour
{
    [Tooltip("Velocidad a la que la moneda gira en su propio eje.")]
    public float velocidadRotacion = 100f;

    [Tooltip("Valor que esta moneda otorga al recolectarse.")]
    public int valorMoneda = 1;

    void Update()
    {
        // Al colocar la velocidad directamente en el eje Y (el segundo valor) y usar Space.World,
        // forzamos a que la moneda gire como un trompo sobre el pavimento.
        transform.Rotate(0f, velocidadRotacion * Time.deltaTime, 0f, Space.World);
    }

    [Tooltip("El efecto de sonido al recoger la moneda")]
    public AudioClip sonidoRecoleccion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador")) 
        {
            GameManager.Instance.SumarMoneda(valorMoneda);

            // Reproduce el sonido en la posición actual antes de destruirse
            if (sonidoRecoleccion != null)
            {
                // Usamos Camera.main.transform.position para que se escuche al 100% de volumen 
                // sin importar qué tan lejos esté la moneda en el mundo 3D.
                AudioSource.PlayClipAtPoint(sonidoRecoleccion, Camera.main.transform.position);
            }

            Destroy(gameObject);
        }
    }
}
