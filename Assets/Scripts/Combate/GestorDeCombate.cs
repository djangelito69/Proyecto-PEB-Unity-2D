using UnityEngine;

public class GestorDeCombate : MonoBehaviour
{
    public Combate jugador;
    public Combate enemigo;

    public bool combateTerminado = false;
    private bool turnoJugador = true;

    void Start()
    {
        ConfigurarJugador();
        ConfigurarEnemigo();
        Debug.Log("=== COMIENZA EL COMBATE ===");
        MostrarStats();
    }

    // ──────────────────────────────────────────
    //  CONFIGURACIÓN DE PERSONAJES
    // ──────────────────────────────────────────

    void ConfigurarJugador()
    {
        jugador = new Combate();

        switch (DatosPersonaje.PersonajeSeleccionado)
        {
            case DatosPersonaje.TipoPersonaje.Gato:
                jugador.Nombre = "Gato";
                jugador.vidaMaxima = 15;
                jugador.vidaActual = 15;
                jugador.PA_Maxima = 10;
                jugador.PA_Actual = 10;
                jugador.dañoBasico = 3;
                jugador.dañoEspecial = 6;
                jugador.PA_costoBasico = 2;
                jugador.PA_costoEspecial = 5;
                jugador.PA_recuperacionPorTurno = 2;
                break;

            case DatosPersonaje.TipoPersonaje.Perro:
                jugador.Nombre = "Perro";
                jugador.vidaMaxima = 20;
                jugador.vidaActual = 20;
                jugador.PA_Maxima = 7;
                jugador.PA_Actual = 7;
                jugador.dañoBasico = 4;
                jugador.dañoEspecial = 7;
                jugador.PA_costoBasico = 2;
                jugador.PA_costoEspecial = 5;
                jugador.PA_recuperacionPorTurno = 2;
                break;

            case DatosPersonaje.TipoPersonaje.Raton:
                jugador.Nombre = "Ratón";
                jugador.vidaMaxima = 10;
                jugador.vidaActual = 10;
                jugador.PA_Maxima = 15;
                jugador.PA_Actual = 15;
                jugador.dañoBasico = 2;
                jugador.dañoEspecial = 8;
                jugador.PA_costoBasico = 1;
                jugador.PA_costoEspecial = 4;
                jugador.PA_recuperacionPorTurno = 3;
                break;
        }
    }

    void ConfigurarEnemigo()
    {
        enemigo = new Combate
        {
            Nombre = "Bote de Basura",
            vidaMaxima = 10,
            vidaActual = 10,
            PA_Maxima = 6,
            PA_Actual = 6,
            dañoBasico = 2,
            PA_costoBasico = 2,
            PA_recuperacionPorTurno = 2
        };
    }

    // ──────────────────────────────────────────
    //  ACCIONES DEL JUGADOR
    // ──────────────────────────────────────────

    public void AtaqueBasicoJugador()
    {
        if (combateTerminado || !turnoJugador) return;

        if (!jugador.TienePAParaBasico())
        {
            Debug.Log("No tienes PA suficiente para el ataque básico.");
            return;
        }

        jugador.GastarPA(jugador.PA_costoBasico);
        enemigo.RecibirDaño(jugador.dañoBasico);
        Debug.Log($"{jugador.Nombre} usó ataque básico → {jugador.dañoBasico} daño a {enemigo.Nombre}");

        if (RevisarGanador()) return;

        PasarTurnoAlEnemigo();
    }

    public void AtaqueEspecialJugador()
    {
        if (combateTerminado || !turnoJugador) return;

        if (!jugador.TienePAParaEspecial())
        {
            Debug.Log("No tienes PA suficiente para el ataque especial.");
            return;
        }

        jugador.GastarPA(jugador.PA_costoEspecial);
        enemigo.RecibirDaño(jugador.dañoEspecial);
        Debug.Log($"{jugador.Nombre} usó ataque especial → {jugador.dañoEspecial} daño a {enemigo.Nombre}");

        if (RevisarGanador()) return;

        PasarTurnoAlEnemigo();
    }

    // ──────────────────────────────────────────
    //  TURNO DEL ENEMIGO
    // ──────────────────────────────────────────

    void PasarTurnoAlEnemigo()
    {
        turnoJugador = false;
        jugador.RecuperarPA();
        TurnoEnemigo();
    }

    void TurnoEnemigo()
    {
        Debug.Log($"─── Turno de {enemigo.Nombre} ───");

        if (!enemigo.TienePAParaBasico())
        {
            Debug.Log($"{enemigo.Nombre} no tiene PA. Recuperando stamina...");
            enemigo.RecuperarPA();
        }
        else
        {
            enemigo.GastarPA(enemigo.PA_costoBasico);
            jugador.RecibirDaño(enemigo.dañoBasico);
            Debug.Log($"{enemigo.Nombre} atacó → {enemigo.dañoBasico} daño a {jugador.Nombre}");
            enemigo.RecuperarPA();
        }

        if (RevisarGanador()) return;

        turnoJugador = true;
        MostrarStats();
        Debug.Log("─── Tu turno ───");
    }

    // ──────────────────────────────────────────
    //  REVISIÓN Y UTILIDADES
    // ──────────────────────────────────────────

    bool RevisarGanador()
    {
        if (!enemigo.EstaVivo)
        {
            combateTerminado = true;
            MostrarStats();
            Debug.Log("¡Ganaste! El enemigo fue derrotado.");
            return true;
        }

        if (!jugador.EstaVivo)
        {
            combateTerminado = true;
            MostrarStats();
            Debug.Log("Perdiste... El jugador fue derrotado.");
            return true;
        }

        return false;
    }

    void MostrarStats()
    {
        Debug.Log(jugador.ObtenerStats());
        Debug.Log(enemigo.ObtenerStats());
    }
}