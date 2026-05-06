using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIExperienciaManager : MonoBehaviour
{
    [Header("UI - Experiencia")]
    [SerializeField] private TextMeshProUGUI textoNivel;
    [SerializeField] private TextMeshProUGUI textoExperiencia;

    [Header("UI - Vida")]
    [SerializeField] private Slider sliderVida;
    [SerializeField] private TextMeshProUGUI textoVida;

    [Header("Configuración")]
    [SerializeField] private float tiempoMaximoEspera = 10f;

    private GestorExperiencia gestorExp;

    void Start()
    {
        StartCoroutine(InicializarConEspera());
    }

    IEnumerator InicializarConEspera()
    {
        float tiempo = 0f;

        // Esperar instancia
        while (GestorExperiencia.instancia == null && tiempo < tiempoMaximoEspera)
        {
            tiempo += Time.deltaTime;
            yield return null;
        }

        if (GestorExperiencia.instancia == null)
        {
            Debug.LogError("GestorExperiencia no encontrado.");
            yield break;
        }

        gestorExp = GestorExperiencia.instancia;

        // Esperar inicialización real
        tiempo = 0f;
        while (!gestorExp.EstaInicializado() && tiempo < tiempoMaximoEspera)
        {
            tiempo += Time.deltaTime;
            yield return null;
        }

        if (!gestorExp.EstaInicializado())
        {
            Debug.LogError("GestorExperiencia no se inicializó.");
            yield break;
        }

        ConfigurarSliders();
        SuscribirseEventos();
        ActualizacionInicial();
    }

    void ConfigurarSliders()
    {
        if (sliderVida != null)
        {
            sliderVida.minValue = 0;
            sliderVida.maxValue = 1;
            sliderVida.interactable = false;
        }
    }

    void SuscribirseEventos()
    {
        gestorExp.OnExperienciaActualizada += ActualizarUIExperiencia;
        gestorExp.OnVidaActualizada += ActualizarBarraVida;
    }

    void ActualizacionInicial()
    {
        ActualizarUIExperiencia(
            gestorExp.ObtenerNivel(),
            gestorExp.ObtenerExperienciaActual(),
            gestorExp.ObtenerExperienciaParaProximo()
        );

        ActualizarBarraVida();
    }

    void ActualizarUIExperiencia(int nivel, int expActual, int expProximo)
    {
        if (gestorExp == null || !gestorExp.EstaInicializado()) return;

        if (textoNivel != null)
            textoNivel.text = $"Nivel {nivel}";

        if (textoExperiencia != null)
            textoExperiencia.text = $"{expActual} / {expProximo}";
    }

    void ActualizarBarraVida()
    {
        if (gestorExp == null || !gestorExp.EstaInicializado()) return;

        int vidaActual = gestorExp.ObtenerVidaActual();
        int vidaMax = gestorExp.ObtenerVidaMaxima();

        if (vidaMax <= 0) return;

        if (textoVida != null)
            textoVida.text = $"{vidaActual} / {vidaMax}";

        if (sliderVida != null)
            sliderVida.value = (float)vidaActual / vidaMax;
    }

    void OnDestroy()
    {
        if (gestorExp != null)
        {
            gestorExp.OnExperienciaActualizada -= ActualizarUIExperiencia;
            gestorExp.OnVidaActualizada -= ActualizarBarraVida;
        }
    }
}