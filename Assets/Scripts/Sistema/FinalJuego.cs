using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class FinalJuego : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoMensaje;
    public TMP_Text textoTitulo;
    public GameObject panelBotones;
    public Image fadePanel;

    [Header("Configuracion")]
    public string nombreJuego = "MI JUEGO";

    [TextArea]
    public string[] mensajesFinales;

    public float duracionFade = 1f;
    public float tiempoEntreMensajes = 3f;

    private void Start()
    {
        panelBotones.SetActive(false);

        StartCoroutine(SecuenciaFinal());
    }

    IEnumerator SecuenciaFinal()
    {
        // Empieza negro
        fadePanel.color = new Color(0, 0, 0, 1);

        textoTitulo.alpha = 0;
        textoMensaje.alpha = 1;

        // Mostrar mensajes
        foreach (string mensaje in mensajesFinales)
        {
            textoMensaje.text = mensaje;

            // Fade In
            yield return StartCoroutine(Fade(1, 0));

            // Esperar
            yield return new WaitForSeconds(tiempoEntreMensajes);

            // Fade Out
            yield return StartCoroutine(Fade(0, 1));
        }

        // Limpiar texto de mensajes
        textoMensaje.text = "";

        // Mostrar titulo final
        textoTitulo.text = nombreJuego;

        // Fade In titulo
        yield return StartCoroutine(Fade(1, 0));

        yield return StartCoroutine(FadeTexto(textoTitulo, 0, 1));

        yield return new WaitForSeconds(2f);

        // Mostrar botones
        panelBotones.SetActive(true);
    }

    IEnumerator Fade(float alphaInicial, float alphaFinal)
    {
        float tiempo = 0;

        Color color = fadePanel.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            float alpha = Mathf.Lerp(
                alphaInicial,
                alphaFinal,
                tiempo / duracionFade
            );

            fadePanel.color = new Color(
                color.r,
                color.g,
                color.b,
                alpha
            );

            yield return null;
        }

        fadePanel.color = new Color(
            color.r,
            color.g,
            color.b,
            alphaFinal
        );
    }

    IEnumerator FadeTexto(TMP_Text texto, float alphaInicial, float alphaFinal)
    {
        float tiempo = 0;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            float alpha = Mathf.Lerp(
                alphaInicial,
                alphaFinal,
                tiempo / duracionFade
            );

            texto.alpha = alpha;

            yield return null;
        }

        texto.alpha = alphaFinal;
    }

    public void VolverMenu()
    {
        if (GestorExperiencia.instancia != null)
        {
            GestorExperiencia.instancia.ReiniciarDatos();
        }

        SceneManager.LoadScene("MenuPrincipal");
    }

    public void SalirJuego()
    {
        Application.Quit();

        Debug.Log("Salir del juego");
    }
}