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
    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Enemigo"))
        {
            Destroy(otro.gameObject); // Destruye al enemigo
            Destroy(gameObject);      // Destruye la bala
        }
    }
}