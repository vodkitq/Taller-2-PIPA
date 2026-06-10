using UnityEngine;
using System.Collections.Generic; 

public class GeneradorSuelos : MonoBehaviour
{
    [Header("Configuración del Mapa")]
    public GameObject[] prefabsSuelos; 
    public Transform jugador;      
    
    public int cantidadPlataformas = 15; 
    public float largoDelSuelo = 2f; 

    private float posicionZGeneracion = 0f;
    private List<GameObject> suelosActivos = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < cantidadPlataformas; i++)
        {
            CrearPlataforma();
        }
    }

    void Update()
    {
        if (jugador == null) return;

        if (jugador.position.z - largoDelSuelo > suelosActivos[0].transform.position.z)
        {
            CrearPlataforma(); 
            BorrarPlataformaAntigua(); 
        }
    }

    void CrearPlataforma()
    {
        int indiceAleatorio = Random.Range(0, prefabsSuelos.Length);
        GameObject sueloElegido = prefabsSuelos[indiceAleatorio];

        GameObject nuevoSuelo = Instantiate(sueloElegido, new Vector3(0, 0, posicionZGeneracion), Quaternion.identity);
        suelosActivos.Add(nuevoSuelo);
        
        posicionZGeneracion += largoDelSuelo;
    }

    void BorrarPlataformaAntigua()
    {
        Destroy(suelosActivos[0]);
        suelosActivos.RemoveAt(0);
    }
}