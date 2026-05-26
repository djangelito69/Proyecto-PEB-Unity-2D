using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Gestor de combate que se ejecuta en la escena de combate.
/// Integra:
/// - Sistema de experiencia
/// - Persistencia de vida
/// - Mejoras dinámicas de stats
/// Se coordina con GestorCombateGlobal para control de transiciones.
/// </summary>
public class GestorDeCombate : MonoBehaviour
{
    public Combate jugador;
    public Combate enemigo;

    public static GestorDeCombate instancia;

    [Header("Enemigo actual")]
    public DatosEnemigos.TipoEnemigo tipoEnemigo;
    public GameObject enemigoEnMapa;

    public bool combateTerminado = false;
    private bool turnoJugador = true;

    private Coroutine corrutinavolverAlMapa;

    public System.Action<string> OnMensajeCombate;
    public System.Action OnDañoRecibidoEnemigo;
    public System.Action OnDañoRecibidoJugador;

    void EnviarMensaje(string mensaje)
    {
        Debug.Log(mensaje);
        OnMensajeCombate?.Invoke(mensaje);
    }

    void Awake()
    {
        instancia = this;

        ConfigurarJugador();
        ConfigurarEnemigo();

        EnviarMensaje("=== COMIENZA EL COMBATE ===");
        MostrarStats();
    }

    void Start()
    {
        // Optimizado: Ya no se busca el AudioListener aquí. 
        // El GestorCombateGlobal se encarga de apagar el del mapa antes de cargar esta escena.
        Debug.Log("GestorDeCombate: Escena de combate iniciada. Audio configurado por el gestor global.");
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
        if (GestorExperiencia.instancia != null)
        {
            DatosCombate datosActualizados = GestorExperiencia.instancia.ObtenerDatosActuales();
            jugador = CrearCombatiente(datosActualizados);

            jugador.vidaActual = GestorExperiencia.instancia.ObtenerVidaActual();
            jugador.PA_Actual = GestorExperiencia.instancia.ObtenerPAActual();

            Debug.Log($"[COMBATE] Jugador configurado: {jugador.Nombre} (Nivel {GestorExperiencia.instancia.ObtenerNivel()}) | Vida: {jugador.vidaActual}/{jugador.vidaMaxima} | PA: {jugador.PA_Actual}/{jugador.PA_Maxima}");
        }
        else
        {
            DatosCombate datos = DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);
            jugador = CrearCombatiente(datos);
            Debug.LogWarning("[COMBATE] GestorExperiencia no encontrado, usando datos base");
        }

        // ✅ Reglas especiales de combate para el jugador:
        // - Ataque básico gratis (0 PA de coste)
        // - Regeneración de exactamente +1 PA por turno durante el combate
        if (jugador != null)
        {
            jugador.PA_costoBasico = 0;
            jugador.PA_recuperacionPorTurno = 1;
        }
    }

    void ConfigurarEnemigo()
    {
        if (GestorEnemigos.instancia != null && GestorEnemigos.instancia.HayEnemigo())
        {
            DatosCombate datos = GestorEnemigos.instancia.ObtenerDatosEnemigo();
            tipoEnemigo = GestorEnemigos.instancia.ObtenerTipoEnemigo();
            enemigo = CrearCombatiente(datos);
            Debug.Log($"[COMBATE] Enemigo: {enemigo.Nombre}");
        }
        else
        {
            Debug.LogWarning("[COMBATE] No se encontró enemigo, usando tipo del Inspector");
            DatosCombate datos = DatosEnemigos.ObtenerDatos(tipoEnemigo);
            enemigo = CrearCombatiente(datos);
        }
    }

    public void AtaqueBasicoJugador()
    {
        if (jugador == null || enemigo == null)
        {
            Debug.LogError("GestorDeCombate: Jugador o enemigo es null en AtaqueBasicoJugador()");
            return;
        }

        if (combateTerminado || !turnoJugador) return;

        if (!jugador.TienePAParaBasico())
        {
            EnviarMensaje("No tienes PA suficiente para el ataque básico.");
            return;
        }

        jugador.GastarPA(jugador.PA_costoBasico);
        enemigo.RecibirDaño(jugador.dañoBasico);
        OnDañoRecibidoEnemigo?.Invoke();
        EnviarMensaje($"{jugador.Nombre} usó ataque básico → {jugador.dañoBasico} daño a {enemigo.Nombre}");

        if (RevisarGanador()) return;
        PasarTurnoAlEnemigo();
    }

    public void AtaqueEspecialJugador()
    {
        if (jugador == null || enemigo == null)
        {
            Debug.LogError("GestorDeCombate: Jugador o enemigo es null en AtaqueEspecialJugador()");
            return;
        }

        if (combateTerminado || !turnoJugador) return;

        if (!jugador.TienePAParaEspecial())
        {
            EnviarMensaje("No tienes PA suficiente para el ataque especial.");
            return;
        }

        jugador.GastarPA(jugador.PA_costoEspecial);
        enemigo.RecibirDaño(jugador.dañoEspecial);
        OnDañoRecibidoEnemigo?.Invoke();
        EnviarMensaje($"{jugador.Nombre} usó ataque especial → {jugador.dañoEspecial} daño a {enemigo.Nombre}");

        if (RevisarGanador()) return;
        PasarTurnoAlEnemigo();
    }

    void PasarTurnoAlEnemigo()
    {
        if (jugador == null || enemigo == null)
        {
            Debug.LogError("GestorDeCombate: Jugador o enemigo es null en PasarTurnoAlEnemigo()");
            return;
        }

        turnoJugador = false;
        jugador.RecuperarPA();
        TurnoEnemigo();
    }

    void TurnoEnemigo()
    {
        if (jugador == null || enemigo == null)
        {
            Debug.LogError("GestorDeCombate: Jugador o enemigo es null en TurnoEnemigo()");
            return;
        }

        EnviarMensaje($"─── Turno de {enemigo.Nombre} ───");

        int decision = Random.Range(0, 100);

        if (enemigo.TienePAParaEspecial() && decision < 30)
        {
            enemigo.GastarPA(enemigo.PA_costoEspecial);
            jugador.RecibirDaño(enemigo.dañoEspecial);
            OnDañoRecibidoJugador?.Invoke();
            EnviarMensaje($"{enemigo.Nombre} usó ataque especial → {enemigo.dañoEspecial} daño");
            enemigo.RecuperarPA();
        }
        else if (enemigo.TienePAParaBasico())
        {
            enemigo.GastarPA(enemigo.PA_costoBasico);
            jugador.RecibirDaño(enemigo.dañoBasico);
            OnDañoRecibidoJugador?.Invoke();
            EnviarMensaje($"{enemigo.Nombre} usó ataque básico → {enemigo.dañoBasico} daño");
            enemigo.RecuperarPA();
        }
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
        Debug.Log("GestorDeCombate: VolverAlMapa() llamado");

        Time.timeScale = 1f;

        if (MusicManager.instancia != null)
        {
            MusicManager.instancia.DetenerMusica();
        }

        // Optimizado: El GestorCombateGlobal reactivará el AudioListener automáticamente
        // a través de su método ReestablecerTransicion().

        if (GestorCombateGlobal.instancia != null)
        {
            GestorCombateGlobal.instancia.ReestablecerTransicion();
            Debug.Log("GestorDeCombate: Flag de transición reestablecido");
        }

        SceneManager.UnloadSceneAsync("Combate");
    }

    private IEnumerator EsperarYVolverAlMapa(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        VolverAlMapa();
    }

    void OnDestroy()
    {
        if (corrutinavolverAlMapa != null)
        {
            StopCoroutine(corrutinavolverAlMapa);
            Debug.Log("GestorDeCombate: Corrutina de retorno detenida");
        }
    }

    bool RevisarGanador()
    {
        if (!enemigo.EstaVivo)
        {
            combateTerminado = true;
            EnviarMensaje("¡Ganaste!");

            if (GestorExperiencia.instancia != null)
            {
                GestorExperiencia.instancia.EstablecerVidaActual(jugador.vidaActual);
                GestorExperiencia.instancia.EstablecerPAActual(jugador.PA_Actual);
            }

            if (GestorEnemigos.instancia != null)
            {
                int expGanada = DatosEnemigos.ObtenerExperiencia(tipoEnemigo);
                EnviarMensaje($"¡Ganaste {expGanada} XP!");

                if (GestorExperiencia.instancia != null)
                {
                    GestorExperiencia.instancia.AñadirExperiencia(expGanada);
                }

                GestorEnemigos.instancia.DestruirEnemigoDelMapa();
                GestorEnemigos.instancia.LimpiarEnemigo();
            }

            // Si no hay UICombateVictoria instalada en la escena, volvemos automáticamente al mapa como fallback.
            // Si la hay, esa interfaz mostrará la pantalla de victoria y esperará a que el jugador pulse "Continuar".
            if (FindObjectOfType<UICombateVictoria>() == null)
            {
                corrutinavolverAlMapa = StartCoroutine(EsperarYVolverAlMapa(1.5f));
            }
            return true;
        }

        if (!jugador.EstaVivo)
        {
            combateTerminado = true;
            MostrarStats();
            EnviarMensaje("Perdiste... El jugador fue derrotado.");

            if (GestorExperiencia.instancia != null)
            {
                GestorExperiencia.instancia.EstablecerVidaActual(jugador.vidaActual);
                GestorExperiencia.instancia.EstablecerPAActual(jugador.PA_Actual);
            }

            if (GestorEnemigos.instancia != null)
            {
                GestorEnemigos.instancia.LimpiarEnemigo();
            }

            // Si no hay UICombateGameOver instalada en la escena, volvemos automáticamente al mapa como fallback.
            // Si la hay, esa interfaz mostrará la pantalla de derrota y esperará a que el jugador elija Reintentar o Menú Principal.
            if (FindObjectOfType<UICombateGameOver>() == null)
            {
                corrutinavolverAlMapa = StartCoroutine(EsperarYVolverAlMapa(1.5f));
            }
            return true;
        }

        return false;
    }

    void MostrarStats()
    {
        if (jugador == null || enemigo == null)
        {
            Debug.LogError("GestorDeCombate: Jugador o enemigo es null en MostrarStats()");
            return;
        }

        EnviarMensaje(jugador.ObtenerStats());
        EnviarMensaje(enemigo.ObtenerStats());
    }
}