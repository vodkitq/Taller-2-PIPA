using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCorrer = 5f;
    public float velocidadLateral = 10f;
    public float distanciaCarril = 1.5f; 
    public float fuerzaSalto = 7f;

    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;

    [Header("Configuración de Disparo")]
    [Tooltip("Tiempo mínimo en segundos entre cada disparo")]
    public float cadenciaDisparo = 0.5f;
    
    // Variable interna para saber en qué segundo exacto podremos volver a disparar
    private float proximoDisparo = 0f;

    private int carrilActual = 1; 
    private Rigidbody rb;
    private bool enElSuelo = true;

    [Header("Efectos de Sonido")]
    public AudioSource audioSourceJugador;
    public AudioClip sonidoSalto;
    public AudioClip sonidoDisparo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. Movimiento constante
        transform.Translate(Vector3.forward * velocidadCorrer * Time.deltaTime);

        // 2. Controles de carril
        if (Input.GetKeyDown(KeyCode.D))
        {
            carrilActual++;
            if (carrilActual > 2) carrilActual = 2; 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            carrilActual--;
            if (carrilActual < 0) carrilActual = 0; 
        }

        // Movimiento suave
        float posicionXDeseada = 0;
        if (carrilActual == 0) posicionXDeseada = -distanciaCarril;
        else if (carrilActual == 1) posicionXDeseada = 0;
        else if (carrilActual == 2) posicionXDeseada = distanciaCarril;

        Vector3 posicionObjetivo = new Vector3(posicionXDeseada, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, velocidadLateral * Time.deltaTime);

        // 3. Salto
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            Saltar();
        }

        // 4. DISPARO (Tecla F)
        // Verificamos si presiona F Y ADEMÁS si el tiempo actual del juego ya superó al tiempo de recarga
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= proximoDisparo)
        {
            // Calculamos cuándo será el próximo disparo permitido (tiempo actual + 0.5 segundos)
            proximoDisparo = Time.time + cadenciaDisparo;
            
            // Llamamos al método que crea la bala y hace el sonido
            Disparar(); 
        }
    }

    void Saltar()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        enElSuelo = false;

        if (audioSourceJugador != null && sonidoSalto != null)
    {
        audioSourceJugador.PlayOneShot(sonidoSalto); // PlayOneShot permite reproducir sonidos superpuestos
    }
    }

    void Disparar()
    {
        // Creamos la bala exactamente en la posición y rotación del PuntoDisparo
        if (prefabBala != null && puntoDisparo != null)
        {
            Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
        }

        if (audioSourceJugador != null && sonidoDisparo != null)
    {
        audioSourceJugador.PlayOneShot(sonidoDisparo);
    }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si toca suelo, puede saltar
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }

        // Si toca Obstaculo o Enemigo, activamos el Game Over
        if (collision.gameObject.CompareTag("Obstaculo") || collision.gameObject.CompareTag("Enemigo"))
        {
            // Llamamos al GameManager en lugar de reiniciar directamente
            GameManager.Instance.GameOver();
        }
    }
}