using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 30f;
    public float tiempoDeVida = 3f; // Segundos antes de desaparecer

    void Start()
    {
        // Programamos su autodestrucción para no llenar la memoria
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        // Mover la bala siempre hacia adelante
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    // Detectar si choca con un enemigo
    private void OnTriggerEnter(Collider other)
    {
        // 1. Si la bala choca con un Zombie
        if (other.CompareTag("Enemigo"))
        {
            // Destruimos al Zombie
            Destroy(other.gameObject); 
            // Destruimos la bala
            Destroy(gameObject); 
        }
        // 2. Si la bala choca con un auto/obstáculo
        else if (other.CompareTag("Obstaculo")) // Usa el Tag exacto que le pusiste a tus autos
        {
            // Solo destruimos la bala (no el auto, el auto es indestructible)
            Destroy(gameObject); 
        }
    }
}