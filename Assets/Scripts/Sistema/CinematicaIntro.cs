using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CinematicaIntro : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text textoIntro;
    public Image fadePanel;

    [Header("Configuracion")]
    public string escenaMapa = "Mapa";

    [TextArea]
    public string[] mensajes;

    public float duracionFade = 1f;
    public float tiempoEntreMensajes = 3f;

    private void Start()
    {
        StartCoroutine(SecuenciaIntro());
    }

    IEnumerator SecuenciaIntro()
    {
        // Fade In
        yield return StartCoroutine(Fade(1, 0));

        // Mostrar mensajes
        foreach (string mensaje in mensajes)
        {
            textoIntro.text = mensaje;

            yield return new WaitForSeconds(tiempoEntreMensajes);
        }

        // Fade Out
        yield return StartCoroutine(Fade(0, 1));

        // Cargar mapa
        SceneManager.LoadScene(escenaMapa);
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
}