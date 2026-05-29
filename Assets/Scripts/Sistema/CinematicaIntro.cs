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
        // Empieza completamente negro
        fadePanel.color = new Color(0, 0, 0, 1);

        foreach (string mensaje in mensajes)
        {
            // Cambiar texto
            textoIntro.text = mensaje;

            // Fade In (de negro a visible)
            yield return StartCoroutine(Fade(1, 0));

            // Esperar mientras se muestra el mensaje
            yield return new WaitForSeconds(tiempoEntreMensajes);

            // Fade Out (de visible a negro)
            yield return StartCoroutine(Fade(0, 1));
        }

        // Cargar siguiente escena
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