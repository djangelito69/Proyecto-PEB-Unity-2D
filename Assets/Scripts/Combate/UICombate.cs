using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Log de batalla")]
    public TextMeshProUGUI textoLog;
    public ScrollRect scrollLog;

    private List<string> historialLog = new List<string>();
    private const int MAX_LINEAS = 20;

    void Start()
    {
        botonAtaqueBasico.onClick.AddListener(OnBasico);
        botonAtaqueEspecial.onClick.AddListener(OnEspecial);
        AgregarLog("=== Comienza el combate ===");
        AgregarLog($"{gestordecombate.enemigo.Nombre} aparece!");
    }

    void OnBasico()
    {
        var j = gestordecombate.jugador;
        var e = gestordecombate.enemigo;

        if (!j.TienePAParaBasico())
        {
            AgregarLog("No tienes PA suficiente.");
            return;
        }

        int vidaAntes = e.vidaActual;
        int vidaJugadorAntes = j.vidaActual;
        gestordecombate.AtaqueBasicoJugador();

        AgregarLog($"{j.Nombre} usó Ataque Básico → {vidaAntes - e.vidaActual} daño");

        if (!e.EstaVivo)
            AgregarLog($"¡{e.Nombre} fue derrotado!");
        else
            RegistrarTurnoEnemigo(vidaJugadorAntes);

        ActualizarUI();
    }

    void OnEspecial()
    {
        var j = gestordecombate.jugador;
        var e = gestordecombate.enemigo;

        if (!j.TienePAParaEspecial())
        {
            AgregarLog("No tienes PA suficiente.");
            return;
        }

        int vidaAntes = e.vidaActual;
        int vidaJugadorAntes = j.vidaActual;
        gestordecombate.AtaqueEspecialJugador();

        AgregarLog($"{j.Nombre} usó Ataque Especial → {vidaAntes - e.vidaActual} daño");

        if (!e.EstaVivo)
            AgregarLog($"¡{e.Nombre} fue derrotado!");
        else
            RegistrarTurnoEnemigo(vidaJugadorAntes);

        ActualizarUI();
    }

    // Recibe la vida del jugador ANTES de que el enemigo atacara
    void RegistrarTurnoEnemigo(int vidaJugadorAntes)
    {
        var j = gestordecombate.jugador;
        var e = gestordecombate.enemigo;

        int daño = vidaJugadorAntes - j.vidaActual;

        if (daño > 0)
            AgregarLog($"{e.Nombre} atacó → {daño} daño a {j.Nombre}");
        else
            AgregarLog($"{e.Nombre} recuperó stamina.");

        if (!j.EstaVivo)
            AgregarLog($"¡{j.Nombre} fue derrotado!");
    }

    void AgregarLog(string mensaje)
    {
        historialLog.Add(mensaje);

        if (historialLog.Count > MAX_LINEAS)
            historialLog.RemoveAt(0);

        textoLog.text = string.Join("\n", historialLog);

        // Scroll automático al último mensaje
        Canvas.ForceUpdateCanvases();
        if (scrollLog != null)
            scrollLog.verticalNormalizedPosition = 0f;
    }

    public void ActualizarUI()
    {
        var j = gestordecombate.jugador;
        var e = gestordecombate.enemigo;

        textoStatsJugador.text = $"{j.Nombre}\nVida: {j.vidaActual}/{j.vidaMaxima}\nPA: {j.PA_Actual}/{j.PA_Maxima}";
        textoStatsEnemigo.text = $"{e.Nombre}\nVida: {e.vidaActual}/{e.vidaMaxima}\nPA: {e.PA_Actual}/{e.PA_Maxima}";

        botonAtaqueBasico.interactable = j.TienePAParaBasico() && !gestordecombate.combateTerminado;
        botonAtaqueEspecial.interactable = j.TienePAParaEspecial() && !gestordecombate.combateTerminado;

        if (gestordecombate.combateTerminado)
            textoMensaje.text = j.EstaVivo ? "¡Ganaste!" : "Perdiste...";
        else
            textoMensaje.text = "Tu turno";
    }
}