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

    public float duracionFade = 1f;

    private void Start()
    {
        panelBotones.SetActive(false);

        StartCoroutine(SecuenciaFinal());
    }

    IEnumerator SecuenciaFinal()
    {
        // Fade inicial
        yield return StartCoroutine(Fade(1, 0));

        // Primer mensaje
        textoMensaje.alpha = 0;
        textoMensaje.text = "Has logrado regresar a casa...";

        yield return StartCoroutine(FadeTexto(textoMensaje, 0, 1));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeTexto(textoMensaje, 1, 0));

        textoMensaje.text = "";

        // Mostrar titulo
        textoTitulo.alpha = 0;
        textoTitulo.text = nombreJuego;

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