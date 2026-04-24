using UnityEngine;

public class GestorDeCombate : MonoBehaviour
{
    public Combate jugador;
    public Combate enemigo;

    [Header("Enemigo actual")]
    public DatosEnemigos.TipoEnemigo tipoEnemigo;

    public bool combateTerminado = false;
    private bool turnoJugador = true;

    public System.Action<string> OnMensajeCombate;
    void EnviarMensaje(string mensaje)
    {
        Debug.Log(mensaje);

        OnMensajeCombate?.Invoke(mensaje);
    }
    void Awake()
    {
        ConfigurarJugador();
        ConfigurarEnemigo();
        EnviarMensaje("=== COMIENZA EL COMBATE ===");
        MostrarStats();
    }

    Combate CrearCombatiente(DatosCombate datos)
    {
        return new Combate
        {
            Nombre = datos.nombre,

            sprite = datos.sprite,

            vidaMaxima = datos.vida,
            vidaActual = datos.vida,

            PA_Maxima = datos.pa,
            PA_Actual = datos.pa,

            dañoBasico = datos.dañoBasico,
            dañoEspecial = datos.dañoEspecial,

            PA_costoBasico = datos.costoBasico,
            PA_costoEspecial = datos.costoEspecial,

            PA_recuperacionPorTurno = datos.recuperacionPA
        };
    }
    void ConfigurarJugador()
    {
        DatosCombate datos =
            DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);

        jugador = CrearCombatiente(datos);
    }

    void ConfigurarEnemigo()
    {
        DatosCombate datos =
            DatosEnemigos.ObtenerDatos(tipoEnemigo);

        enemigo = CrearCombatiente(datos);
    }

    public void AtaqueBasicoJugador()
    {
        if (combateTerminado || !turnoJugador) return;

        if (!jugador.TienePAParaBasico())
        {
            EnviarMensaje("No tienes PA suficiente para el ataque básico.");
            return;
        }

        jugador.GastarPA(jugador.PA_costoBasico);
        enemigo.RecibirDaño(jugador.dañoBasico);
        EnviarMensaje($"{jugador.Nombre} usó ataque básico → {jugador.dañoBasico} daño a {enemigo.Nombre}");
        if (RevisarGanador()) return;

        PasarTurnoAlEnemigo();
    }

    public void AtaqueEspecialJugador()
    {
        if (combateTerminado || !turnoJugador) return;

        if (!jugador.TienePAParaEspecial())
        {
            EnviarMensaje("No tienes PA suficiente para el ataque especial.");
            return;
        }

        jugador.GastarPA(jugador.PA_costoEspecial);
        enemigo.RecibirDaño(jugador.dañoEspecial);
        EnviarMensaje($"{jugador.Nombre} usó ataque especial → {jugador.dañoEspecial} daño a {enemigo.Nombre}");

        if (RevisarGanador()) return;

        PasarTurnoAlEnemigo();
    }

    void PasarTurnoAlEnemigo()
    {
        turnoJugador = false;
        jugador.RecuperarPA();
        TurnoEnemigo();
    }

    void TurnoEnemigo()
    {
        EnviarMensaje($"─── Turno de {enemigo.Nombre} ───");

        int decision = Random.Range(0, 100);

        // Intentar ataque especial
        if (enemigo.TienePAParaEspecial() && decision < 30)
        {
            enemigo.GastarPA(enemigo.PA_costoEspecial);

            jugador.RecibirDaño(enemigo.dañoEspecial);

            EnviarMensaje($"{enemigo.Nombre} usó ataque especial → {enemigo.dañoEspecial} daño");

            enemigo.RecuperarPA();
        }

        // Ataque básico
        else if (enemigo.TienePAParaBasico())
        {
            enemigo.GastarPA(enemigo.PA_costoBasico);

            jugador.RecibirDaño(enemigo.dañoBasico);

            EnviarMensaje($"{enemigo.Nombre} usó ataque básico → {enemigo.dañoBasico} daño");

            enemigo.RecuperarPA();
        }

        // Recuperar PA
        else
        {
            EnviarMensaje($"{enemigo.Nombre} recuperó stamina.");

            enemigo.RecuperarPA();
        }

        if (RevisarGanador()) return;

        turnoJugador = true;

        MostrarStats();

        EnviarMensaje("─── Tu turno ───");
    }

    bool RevisarGanador()
    {
        if (!enemigo.EstaVivo)
        {
            combateTerminado = true;
            MostrarStats();
            EnviarMensaje("¡Ganaste! El enemigo fue derrotado.");
            return true;
        }

        if (!jugador.EstaVivo)
        {
            combateTerminado = true;
            MostrarStats();
            EnviarMensaje("Perdiste... El jugador fue derrotado.");
            return true;
        }

        return false;
    }

    void MostrarStats()
    {
        EnviarMensaje(jugador.ObtenerStats());
        EnviarMensaje(enemigo.ObtenerStats());
    }
}