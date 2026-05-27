using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICombate : MonoBehaviour
{
    [Header("Referencia al BattleManager")]
    public GestorDeCombate gestordecombate;

    [Header("Botones")]
    public Button botonAtaqueBasico;
    public Button botonAtaqueEspecial;
    public Button botonHuir;

    [Header("Estado de combate")]
    public CanvasGroup canvasGroupBotones;
    public TextMeshProUGUI textoTurno;

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
    public Image panelHUD;

    private List<string> historialLog = new List<string>();
    private const int MAX_LINEAS = 20;

    private Coroutine corrutinaDestelloEnemigo;
    private Coroutine corrutinaDestelloJugador;

    void Start()
    {
        botonAtaqueBasico.onClick.AddListener(OnBasico);
        botonAtaqueEspecial.onClick.AddListener(OnEspecial);
        botonHuir.onClick.AddListener(OnHuir);

        if (gestordecombate == null)
        {
            gestordecombate = GestorDeCombate.instancia;
        }

        if (gestordecombate != null)
        {
            gestordecombate.OnDañoRecibidoEnemigo += IndicarDañoEnemigo;
            gestordecombate.OnDañoRecibidoJugador += IndicarDañoJugador;

            // ✅ CRÍTICO: Suscribirse a los cambios de estado
            gestordecombate.OnEstadoCambiado += OnCombatStateChanged;

            gestordecombate.OnMensajeCombate += MostrarMensaje;
        }


        ActualizarUI();
        ActualizarEstadoTurno(true);
    }

    void OnDestroy()
    {
        if (botonAtaqueBasico != null)
        {
            botonAtaqueBasico.onClick.RemoveListener(OnBasico);
        }

        if (botonAtaqueEspecial != null)
        {
            botonAtaqueEspecial.onClick.RemoveListener(OnEspecial);
        }

        if (botonHuir != null)
        {
            botonHuir.onClick.RemoveListener(OnHuir);
        }

        if (gestordecombate != null)
        {
            gestordecombate.OnDañoRecibidoEnemigo -= IndicarDañoEnemigo;
            gestordecombate.OnDañoRecibidoJugador -= IndicarDañoJugador;

            gestordecombate.OnEstadoCambiado -= OnCombatStateChanged;

            gestordecombate.OnMensajeCombate -= MostrarMensaje;
        }

        if (corrutinaDestelloEnemigo != null)
            StopCoroutine(corrutinaDestelloEnemigo);

        if (corrutinaDestelloJugador != null)
            StopCoroutine(corrutinaDestelloJugador);

        Debug.Log("UICombate: Listeners removidos");
    }

    // ✅ NUEVO: Escuchar cambios de estado del combate
    private void OnCombatStateChanged(CombatState previousState, CombatState newState)
    {
        Debug.Log($"[UI] Estado cambió: {previousState} → {newState}");

        switch (newState)
        {
            case CombatState.PlayerTurn:
                Debug.Log("[UI] Activando botones del jugador");
                ActualizarEstadoTurno(true);
                ActualizarUI();
                break;

            case CombatState.ExecutingAction:
                Debug.Log("[UI] Desactivando botones - acción ejecutándose");
                ActualizarEstadoTurno(false, true);
                break;

            case CombatState.EnemyTurn:
                Debug.Log("[UI] Desactivando botones - turno enemigo");
                ActualizarEstadoTurno(false, false);
                break;

            case CombatState.Victory:
                Debug.Log("[UI] ¡Victoria!");
                ActualizarEstadoTurno(false, false);
                break;

            case CombatState.Defeat:
                Debug.Log("[UI] ¡Derrota!");
                ActualizarEstadoTurno(false, false);
                break;
        }
    }

    void OnBasico()
    {
        if (gestordecombate == null)
        {
            Debug.LogError("UICombate: GestorDeCombate es null en OnBasico()");
            return;
        }

        Debug.Log("[UI] OnBasico() llamado");

        // NO LLAMAMOS A ActualizarEstadoTurno AQUÍ
        // Lo hará automáticamente OnCombatStateChanged cuando el estado cambie a ExecutingAction

        MostrarMensaje("Usaste ataque básico");
        gestordecombate.AtaqueBasicoJugador();
    }

    void OnEspecial()
    {
        if (gestordecombate == null)
        {
            Debug.LogError("UICombate: GestorDeCombate es null en OnEspecial()");
            return;
        }

        Debug.Log("[UI] OnEspecial() llamado");

        // NO LLAMAMOS A ActualizarEstadoTurno AQUÍ
        // Lo hará automáticamente OnCombatStateChanged cuando el estado cambie a ExecutingAction

        MostrarMensaje("Usaste ataque especial");
        gestordecombate.AtaqueEspecialJugador();
    }

    void OnHuir()
    {
        if (gestordecombate == null)
        {
            Debug.LogError("UICombate: GestorDeCombate es null en OnHuir()");
            return;
        }

        Debug.Log("[UI] OnHuir() llamado");

        MostrarMensaje("Intentaste huir");

        gestordecombate.IntentarHuir();
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
            textoStatsJugador.text =
                $"{j.Nombre}\n" +
                $"Vida: {j.vidaActual}/{j.vidaMaxima}\n\n" +
                $"PA: {j.PA_Actual}/{j.PA_Maxima}";
        }

        if (textoStatsEnemigo != null)
        {
            textoStatsEnemigo.text =
                $"{e.Nombre}\n" +
                $"Vida: {e.vidaActual}/{e.vidaMaxima}\n\n" +
                $"PA: {e.PA_Actual}/{e.PA_Maxima}";
        }

        if (botonAtaqueBasico != null)
        {
            botonAtaqueBasico.interactable =
                j.TienePAParaBasico() &&
                !gestordecombate.combateTerminado;
        }

        if (botonAtaqueEspecial != null)
        {
            botonAtaqueEspecial.interactable =
                j.TienePAParaEspecial() &&
                !gestordecombate.combateTerminado;
        }

        if (textoMensaje != null)
        {
            if (gestordecombate.combateTerminado)
            {
                textoMensaje.text = j.EstaVivo ? "¡Ganaste!" : "Perdiste...";
            }
        }

        if (imagenEnemigo != null && e.sprite != null)
        {
            imagenEnemigo.sprite = e.sprite;
        }
    }

    // =====================================================
    // ESTADO DEL COMBATE
    // =====================================================

    public void ActualizarEstadoTurno(bool turnoJugador, bool ejecutandoAccion = false)
    {
        Debug.Log($"[UI] ActualizarEstadoTurno - turnoJugador: {turnoJugador}, ejecutandoAccion: {ejecutandoAccion}");

        if (canvasGroupBotones != null)
        {
            canvasGroupBotones.alpha = turnoJugador ? 1f : 0.5f;
            canvasGroupBotones.interactable = turnoJugador;
            canvasGroupBotones.blocksRaycasts = turnoJugador;

            Debug.Log($"[UI] CanvasGroup - interactable: {canvasGroupBotones.interactable}");
        }
    }

    public void MostrarMensaje(string mensaje)
    {
        if (textoMensaje != null)
        {
            textoMensaje.text = mensaje;
        }
    }

    // =====================================================
    // RETROALIMENTACIÓN VISUAL DE DAÑO
    // =====================================================

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

            imagenEnemigo.color = new Color(1f, 0.4f, 0.4f, 1f);

            yield return new WaitForSeconds(1f);

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
        Image panelAUsar = panelHUD != null ? panelHUD : GetComponent<Image>();

        if (panelAUsar != null)
        {
            Color colorOriginalPanel = panelAUsar.color;

            panelAUsar.color = new Color(
                1f,
                0.3f,
                0.3f,
                colorOriginalPanel.a > 0.1f ? colorOriginalPanel.a : 0.8f
            );

            yield return new WaitForSeconds(1f);

            panelAUsar.color = colorOriginalPanel;
        }
    }
}