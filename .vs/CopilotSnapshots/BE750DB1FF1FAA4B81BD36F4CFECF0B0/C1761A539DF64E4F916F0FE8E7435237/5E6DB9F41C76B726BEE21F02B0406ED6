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

    [Header("Barras de vida")]
    public Slider barraVidaJugador;
    public Slider barraVidaEnemigo;

    [Header("Barras de PA")]
    public Slider barraPAJugador;
    public Slider barraPAEnemigo;

    [Header("Imagen enemigo")]
    public Image imagenEnemigo;

    private List<string> historialLog = new List<string>();
    private const int MAX_LINEAS = 20;

    void Start()
    {

        botonAtaqueBasico.onClick.AddListener(OnBasico);
        botonAtaqueEspecial.onClick.AddListener(OnEspecial);

        ActualizarUI();
    }

    void OnBasico()
    {
        gestordecombate.AtaqueBasicoJugador();

        ActualizarUI();
    }

    void OnEspecial()
    {
        gestordecombate.AtaqueEspecialJugador();

        ActualizarUI();
    }

    public void ActualizarUI()
    {
        var j = gestordecombate.jugador;
        var e = gestordecombate.enemigo;

        barraVidaJugador.maxValue = j.vidaMaxima;
        barraVidaJugador.value = j.vidaActual;

        barraVidaEnemigo.maxValue = e.vidaMaxima;
        barraVidaEnemigo.value = e.vidaActual;

        barraPAJugador.maxValue = j.PA_Maxima;
        barraPAJugador.value = j.PA_Actual;

        barraPAEnemigo.maxValue = e.PA_Maxima;
        barraPAEnemigo.value = e.PA_Actual;

        textoStatsJugador.text = $"{j.Nombre}\nVida: {j.vidaActual}/{j.vidaMaxima}\n\nPA: {j.PA_Actual}/{j.PA_Maxima}";
        textoStatsEnemigo.text = $"{e.Nombre}\nVida: {e.vidaActual}/{e.vidaMaxima}\n\nPA: {e.PA_Actual}/{e.PA_Maxima}";

        botonAtaqueBasico.interactable = j.TienePAParaBasico() && !gestordecombate.combateTerminado;
        botonAtaqueEspecial.interactable = j.TienePAParaEspecial() && !gestordecombate.combateTerminado;

        if (gestordecombate.combateTerminado)
            textoMensaje.text = j.EstaVivo ? "¡Ganaste!" : "Perdiste...";
        else
            textoMensaje.text = "Tu turno";

        imagenEnemigo.sprite = e.sprite;
    }
}