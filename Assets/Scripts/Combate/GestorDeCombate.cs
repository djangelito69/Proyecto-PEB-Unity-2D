using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorDeCombate : MonoBehaviour
{
    public Combate jugador;
    public Combate enemigo;

    [Header("Enemigo actual")]
    public DatosEnemigos.TipoEnemigo tipoEnemigo;
    public GameObject enemigoEnMapa;

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
        DatosCombate datos = DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);
        jugador = CrearCombatiente(datos);
    }

    void ConfigurarEnemigo()
    {
        // Intentar obtener el enemigo del EnemyManager
        if (GestorEnemigos.instancia != null && GestorEnemigos.instancia.HayEnemigo())
        {
            DatosCombate datos = GestorEnemigos.instancia.ObtenerDatosEnemigo();
            tipoEnemigo = GestorEnemigos.instancia.ObtenerTipoEnemigo();
            enemigo = CrearCombatiente(datos);
        }
        else
        {
            // Fallback: usar el tipo de enemigo del Inspector (para testing)
            Debug.LogWarning("No se encontró enemigo en EnemyManager, usando tipo del Inspector");
            DatosCombate datos = DatosEnemigos.ObtenerDatos(tipoEnemigo);
            enemigo = CrearCombatiente(datos);
        }
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
    void VolverAlMapa()
    {
        SceneManager.LoadScene("Ciudad_S1");
    }

    bool RevisarGanador()
    {
        if (!enemigo.EstaVivo)
        {
            combateTerminado = true;

            EnviarMensaje("¡Ganaste!");

            GestorEnemigos.instancia.EnemigoDerrotado = true;


            Invoke("VolverAlMapa", 1.5f);
            return true;
        }

        if (!jugador.EstaVivo)
        {
            combateTerminado = true;
            MostrarStats();
            EnviarMensaje("Perdiste... El jugador fue derrotado.");

            Invoke("VolverAlMapa", 1.5f);
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