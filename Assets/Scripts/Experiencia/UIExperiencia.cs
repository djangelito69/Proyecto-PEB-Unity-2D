using TMPro;
using UnityEngine;
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

    [Header("UI - PA")]
    [SerializeField] private Slider sliderPA;
    [SerializeField] private TextMeshProUGUI textoPA;

    [Header("Panel & Tecla")]
    [SerializeField] private GameObject panelExperiencia;
    [SerializeField] private KeyCode teclaToggle = KeyCode.X;

    [Header("Configuración")]
    [SerializeField] private float tiempoMaximoEspera = 10f;

    private GestorExperiencia gestorExp;

    void Start()
    {
        if (panelExperiencia != null)
            panelExperiencia.SetActive(false);

        StartCoroutine(InicializarConEspera());
    }

    void Update()
    {
        if (panelExperiencia == null) return;

        if (Input.GetKeyDown(teclaToggle))
        {
            bool nuevoEstado = !panelExperiencia.activeSelf;
            panelExperiencia.SetActive(nuevoEstado);

            if (nuevoEstado)
            {
                // Actualiza la UI cuando se abre el panel
                ActualizacionInicial();
            }
        }
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

        if (sliderPA != null)
        {
            sliderPA.minValue = 0;
            sliderPA.maxValue = 1;
            sliderPA.interactable = false;
        }
    }

    void SuscribirseEventos()
    {
        gestorExp.OnExperienciaActualizada += ActualizarUIExperiencia;
        gestorExp.OnVidaActualizada += ActualizarBarraVida;
        // El gestor actualmente reutiliza OnVidaActualizada también para cambios de PA
        gestorExp.OnVidaActualizada += ActualizarBarraPA;
    }

    void ActualizacionInicial()
    {
        ActualizarUIExperiencia(
            gestorExp.ObtenerNivel(),
            gestorExp.ObtenerExperienciaActual(),
            gestorExp.ObtenerExperienciaParaProximo()
        );

        ActualizarBarraVida();
        ActualizarBarraPA();
    }

    void ActualizarUIExperiencia(int nivel, int expActual, int expProximo)
    {
        if (gestorExp == null || !gestorExp.EstaInicializado()) return;

        if (textoNivel != null)
            textoNivel.text = $"Nivel: {nivel}";

        if (textoExperiencia != null)
            textoExperiencia.text = $"XP: {expActual} / {expProximo}";
    }

    void ActualizarBarraVida()
    {
        if (gestorExp == null || !gestorExp.EstaInicializado()) return;

        int vidaActual = gestorExp.ObtenerVidaActual();
        int vidaMax = gestorExp.ObtenerVidaMaxima();

        if (vidaMax <= 0) return;

        if (textoVida != null)
            textoVida.text = $"Vida: {vidaActual} / {vidaMax}";

        if (sliderVida != null)
            sliderVida.value = (float)vidaActual / vidaMax;
    }

    void ActualizarBarraPA()
    {
        if (gestorExp == null || !gestorExp.EstaInicializado()) return;

        int paActual = gestorExp.ObtenerPAActual();

        var datos = gestorExp.ObtenerDatosActuales();
        int paMax = datos != null ? datos.pa : 0;

        if (paMax <= 0) return;

        if (textoPA != null)
            textoPA.text = $"PA: {paActual} / {paMax}";

        if (sliderPA != null)
            sliderPA.value = (float)paActual / paMax;
    }

    void OnDestroy()
    {
        if (gestorExp != null)
        {
            gestorExp.OnExperienciaActualizada -= ActualizarUIExperiencia;
            gestorExp.OnVidaActualizada -= ActualizarBarraVida;
            gestorExp.OnVidaActualizada -= ActualizarBarraPA;
        }
    }
}