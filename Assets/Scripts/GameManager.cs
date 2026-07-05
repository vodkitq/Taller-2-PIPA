using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager implementa el patrón Singleton.
/// Controla el flujo general del juego, el puntaje por distancia, las monedas y el menú de derrota.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Instancia global accesible desde cualquier otro script
    public static GameManager Instance { get; private set; }

    [Header("Referencias del Jugador")]
    [Tooltip("Referencia al Transform del jugador para calcular la distancia recorrida.")]
    public Transform jugadorTransform;
    private float posicionInicialZ; // Punto de partida para calcular la distancia neta

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoMonedas;
    public GameObject menuDerrotaPanel;

    [Header("Audio")]
    public AudioSource audioSourceMusicaFondo;
    public AudioClip musicaDerrota;

    // Variables internas de estado
    private int cantidadMonedas = 0;
    private float puntajeActual = 0f;
    private bool juegoActivo = true;

    private void Awake()
    {
        // Configuración del patrón Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Inicializamos la UI, ocultamos el menú y guardamos la posición inicial del jugador
        menuDerrotaPanel.SetActive(false);
        ActualizarUIMonedas();
        
        if (jugadorTransform != null)
        {
            posicionInicialZ = jugadorTransform.position.z;
        }

        // Aseguramos que el tiempo corra normalmente al inicio
        Time.timeScale = 1f; 
        juegoActivo = true;
    }

    private void Update()
    {
        // Si el juego está activo y tenemos la referencia del jugador, calculamos el puntaje
        if (juegoActivo && jugadorTransform != null)
        {
            CalcularPuntajePorDistancia();
        }
    }

    /// <summary>
    /// Calcula el puntaje basándose en la distancia en el eje Z que ha recorrido el jugador.
    /// </summary>
    private void CalcularPuntajePorDistancia()
    {
        // El puntaje es la diferencia entre la posición Z actual y la inicial
        puntajeActual = jugadorTransform.position.z - posicionInicialZ;
        
        // Evitamos que el puntaje sea negativo si el jugador retrocede por alguna física extraña
        if (puntajeActual < 0) puntajeActual = 0;

        // Formateamos el texto para que tenga ceros a la izquierda (ej: 00000345)
        textoPuntaje.text = "PUNTUACIÓN: " + Mathf.FloorToInt(puntajeActual).ToString("D8");
    }

    /// <summary>
    /// Método público para incrementar el contador de monedas desde el script Moneda.
    /// </summary>
    public void SumarMoneda(int cantidad)
    {
        if (!juegoActivo) return; // Si ya perdió, no suma monedas
        
        cantidadMonedas += cantidad;
        ActualizarUIMonedas();
    }

    private void ActualizarUIMonedas()
    {
        // Formateamos las monedas para que tenga 2 dígitos (ej: 09)
        textoMonedas.text = "MONEDAS: " + cantidadMonedas.ToString("D2");
    }

    /// <summary>
    /// Se llama cuando el jugador colisiona con un obstáculo o enemigo.
    /// Detiene el juego y muestra el menú de derrota.
    /// </summary>
    public void GameOver()
    {
        juegoActivo = false;
        
        // Detenemos el movimiento del juego (pausa las físicas y el Update que dependa de Time.deltaTime)
        Time.timeScale = 0f; 

        // Mostramos el menú
        menuDerrotaPanel.SetActive(true);

        // Cambiamos la música
        if (audioSourceMusicaFondo != null && musicaDerrota != null)
        {
            audioSourceMusicaFondo.Stop();
            audioSourceMusicaFondo.PlayOneShot(musicaDerrota);
        }
    }

    /// <summary>
    /// Método público asignado al botón "Reiniciar" del Menú de Derrota.
    /// Recarga la escena actual.
    /// </summary>
    public void ReiniciarJuego()
    {
        // Restauramos el tiempo antes de reiniciar
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Modifica el volumen maestro de todo el juego (Música y Efectos de Sonido).
    /// Ideal para conectarlo a un Slider de la Interfaz de Usuario (UI).
    /// </summary>
    /// <param name="volumen">Valor flotante entre 0.0f (silencio) y 1.0f (máximo).</param>
    public void CambiarVolumenGlobal(float volumen)
    {
        // AudioListener.volume controla el volumen global de la escena.
        // Usamos Mathf.Clamp01 para asegurarnos de que el valor nunca sea menor a 0 ni mayor a 1 por error.
        AudioListener.volume = Mathf.Clamp01(volumen);
    }
}
