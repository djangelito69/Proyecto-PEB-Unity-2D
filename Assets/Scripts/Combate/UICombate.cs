using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UICombate : MonoBehaviour
{
    [Header("Referencia al BattleManager")]
    public GestorDeCombate gestordecombate;

    [Header("Botones")]
    public Button botonAtaqueBasico;
    public Button botonAtaqueEspecial;

    [Header("Textos de stats")]
    public TextMeshProUGUI textoStatsJugador;
    public TextMeshProUGUI textoStatsEnemigo;
    public TextMeshProUGUI textoMensaje;

    [Header("Barras de vida")]
    public Slider barraVidaJugador;
    public Slider barraVidaEnemigo;

    [Header("Barras de PA")]
    public Slider barraPAJugador;
    public Slider barraPAEnemigo;

    [Header("Imagen enemigo")]
    public Image imagenEnemigo;

    [Header("Paneles de Daño")]
    public Image panelHUD; // Arrastra el panel/fondo del HUD del jugador aquí en el Inspector

    private List<string> historialLog = new List<string>();
    private const int MAX_LINEAS = 20;

    private Coroutine corrutinaDestelloEnemigo;
    private Coroutine corrutinaDestelloJugador;

    void Start()
    {
        botonAtaqueBasico.onClick.AddListener(OnBasico);
        botonAtaqueEspecial.onClick.AddListener(OnEspecial);

        // Obtener referencia al gestor de combate si no se asignó en el Inspector
        if (gestordecombate == null)
        {
            gestordecombate = GestorDeCombate.instancia;
        }

        // Suscribirse a los eventos de daño para disparar la retroalimentación visual
        if (gestordecombate != null)
        {
            gestordecombate.OnDañoRecibidoEnemigo += IndicarDañoEnemigo;
            gestordecombate.OnDañoRecibidoJugador += IndicarDañoJugador;
        }

        ActualizarUI();
    }

    void OnDestroy()
    {
        // ✅ CRÍTICO: Remover listeners para evitar acumulación
        if (botonAtaqueBasico != null)
        {
            botonAtaqueBasico.onClick.RemoveListener(OnBasico);
        }

        if (botonAtaqueEspecial != null)
        {
            botonAtaqueEspecial.onClick.RemoveListener(OnEspecial);
        }

        // Desuscribirse de eventos de daño
        if (gestordecombate != null)
        {
            gestordecombate.OnDañoRecibidoEnemigo -= IndicarDañoEnemigo;
            gestordecombate.OnDañoRecibidoJugador -= IndicarDañoJugador;
        }

        // Detener corrutinas pendientes para evitar fallos de referencia
        if (corrutinaDestelloEnemigo != null) StopCoroutine(corrutinaDestelloEnemigo);
        if (corrutinaDestelloJugador != null) StopCoroutine(corrutinaDestelloJugador);

        Debug.Log("UICombate: Listeners removidos");
    }

    void OnBasico()
    {
        if (gestordecombate == null)
        {
            Debug.LogError("UICombate: GestorDeCombate es null en OnBasico()");
            return;
        }

        gestordecombate.AtaqueBasicoJugador();
        ActualizarUI();
    }

    void OnEspecial()
    {
        if (gestordecombate == null)
        {
            Debug.LogError("UICombate: GestorDeCombate es null en OnEspecial()");
            return;
        }

        gestordecombate.AtaqueEspecialJugador();
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (gestordecombate == null)
        {
            Debug.LogError("UICombate: GestorDeCombate es null en ActualizarUI()");
            return;
        }

        if (gestordecombate.jugador == null || gestordecombate.enemigo == null)
        {
            Debug.LogError("UICombate: Jugador o enemigo es null");
            return;
        }

        var j = gestordecombate.jugador;
        var e = gestordecombate.enemigo;

        // ✅ Validación defensiva para evitar crashes
        if (barraVidaJugador != null)
        {
            barraVidaJugador.maxValue = j.vidaMaxima > 0 ? j.vidaMaxima : 1;
            barraVidaJugador.value = Mathf.Clamp(j.vidaActual, 0, j.vidaMaxima);
        }

        if (barraVidaEnemigo != null)
        {
            barraVidaEnemigo.maxValue = e.vidaMaxima > 0 ? e.vidaMaxima : 1;
            barraVidaEnemigo.value = Mathf.Clamp(e.vidaActual, 0, e.vidaMaxima);
        }

        if (barraPAJugador != null)
        {
            barraPAJugador.maxValue = j.PA_Maxima > 0 ? j.PA_Maxima : 1;
            barraPAJugador.value = Mathf.Clamp(j.PA_Actual, 0, j.PA_Maxima);
        }

        if (barraPAEnemigo != null)
        {
            barraPAEnemigo.maxValue = e.PA_Maxima > 0 ? e.PA_Maxima : 1;
            barraPAEnemigo.value = Mathf.Clamp(e.PA_Actual, 0, e.PA_Maxima);
        }

        if (textoStatsJugador != null)
        {
            textoStatsJugador.text = $"{j.Nombre}\nVida: {j.vidaActual}/{j.vidaMaxima}\n\nPA: {j.PA_Actual}/{j.PA_Maxima}";
        }

        if (textoStatsEnemigo != null)
        {
            textoStatsEnemigo.text = $"{e.Nombre}\nVida: {e.vidaActual}/{e.vidaMaxima}\n\nPA: {e.PA_Actual}/{e.PA_Maxima}";
        }

        if (botonAtaqueBasico != null)
        {
            botonAtaqueBasico.interactable = j.TienePAParaBasico() && !gestordecombate.combateTerminado;
        }

        if (botonAtaqueEspecial != null)
        {
            botonAtaqueEspecial.interactable = j.TienePAParaEspecial() && !gestordecombate.combateTerminado;
        }

        if (textoMensaje != null)
        {
            if (gestordecombate.combateTerminado)
                textoMensaje.text = j.EstaVivo ? "¡Ganaste!" : "Perdiste...";
            else
                textoMensaje.text = "Tu turno";
        }

        if (imagenEnemigo != null && e.sprite != null)
        {
            imagenEnemigo.sprite = e.sprite;
        }
    }

    // ========== RETROALIMENTACIÓN VISUAL DE DAÑO ==========

    public void IndicarDañoEnemigo()
    {
        if (corrutinaDestelloEnemigo != null)
        {
            StopCoroutine(corrutinaDestelloEnemigo);
        }
        corrutinaDestelloEnemigo = StartCoroutine(DestellarEnemigoRojo());
    }

    private IEnumerator DestellarEnemigoRojo()
    {
        if (imagenEnemigo != null)
        {
            Color colorOriginalEnemigo = imagenEnemigo.color;
            // Teñir de un color rojo suave
            imagenEnemigo.color = new Color(1f, 0.4f, 0.4f, 1f);
            yield return new WaitForSeconds(1f);
            // Restaurar color original
            imagenEnemigo.color = colorOriginalEnemigo;
        }
    }

    public void IndicarDañoJugador()
    {
        if (corrutinaDestelloJugador != null)
        {
            StopCoroutine(corrutinaDestelloJugador);
        }
        corrutinaDestelloJugador = StartCoroutine(DestellarHUDJugadorRojo());
    }

    private IEnumerator DestellarHUDJugadorRojo()
    {
        // Si no se asignó un panelHUD en el Inspector, intentamos teñir la imagen de este componente como fallback
        Image panelAUsar = panelHUD != null ? panelHUD : GetComponent<Image>();
        if (panelAUsar != null)
        {
            Color colorOriginalPanel = panelAUsar.color;
            // Teñir el fondo de un rojo traslúcido
            panelAUsar.color = new Color(1f, 0.3f, 0.3f, colorOriginalPanel.a > 0.1f ? colorOriginalPanel.a : 0.8f);
            yield return new WaitForSeconds(1f);
            // Restaurar color original
            panelAUsar.color = colorOriginalPanel;
        }
    }
}