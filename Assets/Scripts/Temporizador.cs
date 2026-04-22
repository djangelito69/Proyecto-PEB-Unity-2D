using TMPro;
using UnityEngine;

public class Temporizador : MonoBehaviour
{
    public float tiempoTotal = 120f;
    private float tiempoActual;

    public int puntaje = 0;
    public int puntajeMinimo = 100;

    public TMP_Text textoTiempo;

    // Paneles
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    public TMP_Text textoPuntajeFinalVictoria;
    public TMP_Text textoPuntajeFinalDerrota;

    private bool juegoTerminado = false;

    void Start()
    {
        tiempoActual = tiempoTotal;

        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
    }

    void Update()
    {
        if (juegoTerminado) return;

        tiempoActual -= Time.deltaTime;

        textoTiempo.text = "Tiempo: " + Mathf.Ceil(tiempoActual).ToString();

        if (tiempoActual <= 0)
        {
            TerminarJuego();
        }
    }

    void TerminarJuego()
    {
        juegoTerminado = true;

        // Detener el tiempo del juego
        Time.timeScale = 0f;

        if (puntaje >= puntajeMinimo)
        {
            panelVictoria.SetActive(true);
            textoPuntajeFinalVictoria.text = "Puntaje: " + puntaje;
        }
        else
        {
            panelDerrota.SetActive(true);
            textoPuntajeFinalDerrota.text = "Puntaje: " + puntaje;
        }
    }

    // Para que otros scripts sumen puntos
    public void SumarPuntos(int cantidad)
    {
        puntaje += cantidad;
    }
}
