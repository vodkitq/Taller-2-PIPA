using UnityEngine;

public class GeneradorObjetos : MonoBehaviour
{
    [Header("Puntos de Aparición")]
    public GameObject puntoIzq;
    public GameObject puntoCen;
    public GameObject puntoDer;

    [Header("Actores (Prefabs)")]
    public GameObject[] autos; 
    public GameObject moneda;
    public GameObject zombie;

    void Start()
    {
        // Solo generamos obstáculos si la calle no es de las primeras (por ejemplo, si su Z > 10)
        if (transform.position.z > 10f) 
        {
            GenerarEnCarril(puntoIzq);
            GenerarEnCarril(puntoCen);
            GenerarEnCarril(puntoDer);
        }
    }

    void GenerarEnCarril(GameObject punto)
    {
        if (punto == null) return;

        // Lanzamos el dado
        int probabilidad = Random.Range(0, 10);

        if (probabilidad == 1 && autos.Length > 0)
        {
            int autoAzar = Random.Range(0, autos.Length);
            Instantiate(autos[autoAzar], punto.transform.position, punto.transform.rotation, transform);
        }
        else if (probabilidad == 2 && moneda != null)
        {
            // Creamos un vector que sube la moneda 0.5 unidades en el eje Y
            Vector3 alturaExtra = new Vector3(0f, 0.3f, 0f); // Ajusta el 0.5f si necesitas más o menos altura

            // Se lo sumamos a punto.transform.position
            Instantiate(moneda, punto.transform.position + alturaExtra, moneda.transform.rotation, transform);
        }
        else if (probabilidad == 3 && zombie != null)
        {
            Instantiate(zombie, punto.transform.position, punto.transform.rotation, transform);
        }
    }
}