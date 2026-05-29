using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GestorDeCombate : MonoBehaviour
{
    [Header("=== COMBATIENTES ===")]
    public Combate jugador;
    public Combate enemigo;

    [Header("=== TIMING (segundos) ===")]
    [SerializeField] private float delayEntreAcciones = 0.5f;
    [SerializeField] private float delayEntreAnticipoYGolpe = 0.3f;
    [SerializeField] private float delayDespuesDeGolpe = 0.5f;
    [SerializeField] private float delayAntesDeTurnoEnemigo = 0.8f;
    [SerializeField] private float delayDespuesDeTurno = 0.5f;
    [SerializeField] private float delayAntesDeTurnoJugador = 0.8f;

    [Header("=== REFERENCIAS ===")]
    public GameObject enemigoEnMapa;
    public DatosEnemigos.TipoEnemigo tipoEnemigo;

    [Header("=== HUIDA ===")]
    [SerializeField] private int probabilidadHuir = 25;

    public static GestorDeCombate instancia;

    // State Machine
    private CombatStateMachine stateMachine;

    // Control de flujo
    public bool combateTerminado = false;
    private Coroutine corrutinavolverAlMapa;

    // Eventos
    public System.Action<string> OnMensajeCombate;
    public System.Action OnDañoRecibidoEnemigo;
    public System.Action OnDañoRecibidoJugador;
    public System.Action<CombatState, CombatState> OnEstadoCambiado;

    #region === INICIALIZACIÓN ===

    [Header("=== AUDIO ===")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip sonidoAtaqueBasicoJugador;
    [SerializeField] private AudioClip sonidoAtaqueEspecialJugador;

    [SerializeField] private AudioClip sonidoAtaqueBasicoEnemigo;
    [SerializeField] private AudioClip sonidoAtaqueEspecialEnemigo;

    [SerializeField] private AudioClip sonidoMuerteJugador;
    [SerializeField] private AudioClip sonidoMuerteEnemigo;
    void Awake()
    {
        instancia = this;

        // Inicializar state machine
        stateMachine = new CombatStateMachine();
        stateMachine.OnStateChanged += OnStateMachineChanged;

        ConfigurarJugador();
        ConfigurarEnemigo();

        EnviarMensaje("=== COMIENZA EL COMBATE ===");
        MostrarStats();
    }

    void Start()
    {
        StartCoroutine(IniciarCombate());
    }

    private IEnumerator IniciarCombate()
    {
        yield return new WaitForSeconds(1f);

        bool empiezaJugador =
            Random.Range(0, 100) < 50;

        if (empiezaJugador)
        {
            stateMachine.SetState(CombatState.PlayerTurn);

            EnviarMensaje("¡Empiezas tú!");
            EnviarMensaje("Tu turno");
        }
        else
        {
            stateMachine.SetState(CombatState.EnemyTurn);

            EnviarMensaje($"¡{enemigo.Nombre} empieza primero!");

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(TurnoEnemigoPK());
        }
    }

    private void OnStateMachineChanged(CombatState previousState, CombatState newState)
    {
        Debug.Log($"[STATE CHANGE] {previousState} → {newState} | CanPlayerAct: {stateMachine.CanPlayerAct()}");
        OnEstadoCambiado?.Invoke(previousState, newState);
    }

    #endregion

    #region === CONFIGURACIÓN ===

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

            Debug.Log($"[COMBATE] Jugador: {jugador.Nombre} | Vida: {jugador.vidaActual}/{jugador.vidaMaxima} | PA: {jugador.PA_Actual}/{jugador.PA_Maxima}");
        }
        else
        {
            DatosCombate datos = DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);
            jugador = CrearCombatiente(datos);
        }

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
        }
        else
        {
            DatosCombate datos = DatosEnemigos.ObtenerDatos(tipoEnemigo);
            enemigo = CrearCombatiente(datos);
        }
    }

    #endregion

    #region === ACCIONES DEL JUGADOR (BLOQUEADAS SI NO ES SU TURNO) ===

    /// <summary>
    /// El jugador intenta hacer un ataque básico.
    /// Solo funciona si:
    /// 1. El estado es PlayerTurn
    /// 2. El combate no ha terminado
    /// 3. Tiene PA suficiente
    /// </summary>
    public void AtaqueBasicoJugador()
    {
        Debug.Log($"[ATAQUE BÁSICO] Estado actual: {stateMachine.CurrentState} | CanPlayerAct: {stateMachine.CanPlayerAct()}");

        if (!stateMachine.CanPlayerAct())
        {
            return;
        }

        if (combateTerminado)
            return;

        if (!jugador.TienePAParaBasico())
        {
            EnviarMensaje("No tienes PA suficiente para el ataque básico.");
            return;
        }


        StartCoroutine(SecuenciaAtaqueJugador(
            "Ataque Básico",
            jugador.dañoBasico,
            jugador.PA_costoBasico
        ));
    }


    /// <summary>
    /// El jugador intenta hacer un ataque especial.
    /// </summary>
    public void AtaqueEspecialJugador()
    {
        if (!stateMachine.CanPlayerAct())
        {
            return;
        }

        if (combateTerminado)
            return;

        if (!jugador.TienePAParaEspecial())
        {
            EnviarMensaje("No tienes PA suficiente.");
            return;
        }

        StartCoroutine(SecuenciaAtaqueJugador(
            "Ataque Especial",
            jugador.dañoEspecial,
            jugador.PA_costoEspecial
        ));
    }

    #endregion

    #region === EJECUCIÓN DE ACCIONES CON FLUJO ===

    /// <summary>
    /// Ejecuta una acción del jugador con flujo completo:
    /// PlayerTurn → ExecutingAction → EnemyTurn → PlayerTurn
    /// </summary>


    private IEnumerator SecuenciaAtaqueJugador(string nombreAtaque, int daño, int costoPA)
    {
        stateMachine.SetState(CombatState.ExecutingAction);

        // MENSAJE DEL ATAQUE
        EnviarMensaje($"{jugador.Nombre} usó {nombreAtaque}");

        yield return new WaitForSeconds(0.8f);

        if (nombreAtaque == "Ataque Básico")
        {
            ReproducirSonido(sonidoAtaqueBasicoJugador);
        }
        else
        {
            ReproducirSonido(sonidoAtaqueEspecialJugador);
        }

        // GASTAR PA
        jugador.GastarPA(costoPA);

        yield return new WaitForSeconds(0.4f);

        // EFECTO VISUAL
        OnDañoRecibidoEnemigo?.Invoke();

        yield return new WaitForSeconds(0.3f);

        // APLICAR DAÑO
        enemigo.RecibirDaño(daño);

        // ACTUALIZAR UI
        FindFirstObjectByType<UICombate>()?.ActualizarUI();

        EnviarMensaje($"{enemigo.Nombre} recibió {daño} de daño");

        yield return new WaitForSeconds(1f);

        jugador.RecuperarPA();

        if (RevisarGanador())
            yield break;

        yield return StartCoroutine(TurnoEnemigoPK());
    }

    private IEnumerator SecuenciaHuir()
    {
        stateMachine.SetState(CombatState.ExecutingAction);

        EnviarMensaje($"{jugador.Nombre} intentó huir...");

        yield return new WaitForSeconds(1f);

        int tirada = Random.Range(0, 100);

        bool huidaExitosa = tirada < probabilidadHuir;

        if (huidaExitosa)
        {
            EnviarMensaje("¡Lograste huir!");

            yield return new WaitForSeconds(0.8f);

            GestorEnemigos.instancia?.DestruirEnemigoDelMapa();

            enemigo.vidaActual = 0;

            FindFirstObjectByType<UICombate>()?.ActualizarUI();

            RevisarGanador();
            yield break;
        }

        EnviarMensaje("No pudiste huir.");

        yield return new WaitForSeconds(1f);

        // EL TURNO PASA AL ENEMIGO
        yield return StartCoroutine(TurnoEnemigoPK());
    }

    private IEnumerator SecuenciaConsumible(int index)
    {
        stateMachine.SetState(CombatState.ExecutingAction);

        if (Inventario.instancia == null)
        {
            stateMachine.SetState(CombatState.PlayerTurn);
            yield break;
        }

        if (index < 0 ||
            index >= Inventario.instancia.consumibles.Count)
        {
            stateMachine.SetState(CombatState.PlayerTurn);
            yield break;
        }

        Consumible consumible =
            Inventario.instancia.consumibles[index];

        string mensaje = $"Usaste {consumible.nombre}";

        if (consumible.curacionVida > 0)
        {
            mensaje +=
                $" | +{consumible.curacionVida} HP";
        }

        if (consumible.recuperacionPA > 0)
        {
            mensaje +=
                $" | +{consumible.recuperacionPA} PA";
        }

        EnviarMensaje(mensaje);

        yield return new WaitForSeconds(0.8f);

        bool usado =
            Inventario.instancia.UsarConsumibleEnCombate(
                index,
                jugador
            );

        if (!usado)
        {
            stateMachine.SetState(CombatState.PlayerTurn);
            yield break;
        }

        // GUARDAR VIDA Y PA
        if (GestorExperiencia.instancia != null)
        {
            GestorExperiencia.instancia
                .EstablecerVidaActual(jugador.vidaActual);

            GestorExperiencia.instancia
                .EstablecerPAActual(jugador.PA_Actual);
        }

        // ACTUALIZAR UI
        FindFirstObjectByType<UICombate>()?.ActualizarUI();

        FindFirstObjectByType<UIInventarioCombate>()
            ?.ActualizarUI();

        yield return new WaitForSeconds(1f);

        // PASAR TURNO
        yield return StartCoroutine(TurnoEnemigoPK());
    }

    private IEnumerator TurnoEnemigoPK()
    {
        stateMachine.SetState(CombatState.EnemyTurn);

        yield return new WaitForSeconds(1f);

        EnviarMensaje($"Turno de {enemigo.Nombre}");

        yield return new WaitForSeconds(1f);

        bool usarEspecial =
            enemigo.TienePAParaEspecial() &&
            Random.Range(0, 100) < 30;

        int daño;
        int costo;
        string nombreAtaque;

        if (usarEspecial)
        {
            daño = enemigo.dañoEspecial;
            costo = enemigo.PA_costoEspecial;
            nombreAtaque = "Ataque Especial";
        }
        else
        {
            daño = enemigo.dañoBasico;
            costo = enemigo.PA_costoBasico;
            nombreAtaque = "Ataque Básico";
        }

        EnviarMensaje($"{enemigo.Nombre} usó {nombreAtaque}");

        yield return new WaitForSeconds(0.8f);

        if (usarEspecial)
        {
            ReproducirSonido(sonidoAtaqueEspecialEnemigo);
        }
        else
        {
            ReproducirSonido(sonidoAtaqueBasicoEnemigo);
        }

        enemigo.GastarPA(costo);

        FindFirstObjectByType<UICombate>()?.ActualizarUI();

        yield return new WaitForSeconds(0.4f);

        OnDañoRecibidoJugador?.Invoke();

        yield return new WaitForSeconds(0.3f);

        jugador.RecibirDaño(daño);

        FindFirstObjectByType<UICombate>()?.ActualizarUI();

        EnviarMensaje($"{jugador.Nombre} recibió {daño} de daño");

        yield return new WaitForSeconds(1f);

        enemigo.RecuperarPA();

        if (RevisarGanador())
            yield break;

        stateMachine.SetState(CombatState.PlayerTurn);

        EnviarMensaje("Tu turno");

        FindFirstObjectByType<UICombate>()?.ActualizarUI();
    }

    #endregion

    #region === TURNO DEL ENEMIGO ===

    /// <summary>
    /// Turno del enemigo con retrasos y animaciones.
    /// </summary>

    #endregion

    #region === REVISIÓN DE CONDICIÓN DE VICTORIA ===

    bool RevisarGanador()
    {
        if (!enemigo.EstaVivo)
        {
            ReproducirSonido(sonidoMuerteEnemigo);

            combateTerminado = true;
            stateMachine.SetState(CombatState.Victory);
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

            if (FindObjectOfType<UICombateVictoria>() == null)
            {
                corrutinavolverAlMapa = StartCoroutine(EsperarYVolverAlMapa(1.5f));
            }
            return true;
        }

        if (!jugador.EstaVivo)
        {
            ReproducirSonido(sonidoMuerteJugador);

            combateTerminado = true;
            stateMachine.SetState(CombatState.Defeat);
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

            if (FindObjectOfType<UICombateGameOver>() == null)
            {
                corrutinavolverAlMapa = StartCoroutine(EsperarYVolverAlMapa(1.5f));
            }
            return true;
        }

        return false;
    }

    #endregion

    #region === TRANSICIÓN DE ESCENA ===

    // DESPUÉS (correcto):
    void VolverAlMapa()
    {
        Time.timeScale = 1f;

        ControladorMusicaEscena controlador = FindFirstObjectByType<ControladorMusicaEscena>();
        if (controlador != null)
        {
            Debug.Log($"[MUSICA] Controlador encontrado en escena: {controlador.gameObject.scene.name}");
            controlador.ReproducirMiMusica();
        }
        else
        {
            Debug.Log("[MUSICA] No se encontró ningún ControladorMusicaEscena");
        }

        SceneManager.UnloadSceneAsync("Combate");
    }

    private IEnumerator EsperarYVolverAlMapa(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        VolverAlMapa();
    }

    #endregion

    #region === UTILIDADES ===

    void EnviarMensaje(string mensaje)
    {
        Debug.Log(mensaje);
        OnMensajeCombate?.Invoke(mensaje);
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }


    public void IntentarHuir()
    {
        if (!stateMachine.CanPlayerAct())
        {
            return;
        }

        if (combateTerminado)
            return;

        StartCoroutine(SecuenciaHuir());
    }

    public void UsarConsumibleCombate(int index)
    {
        if (!stateMachine.CanPlayerAct())
        {
            return;
        }

        if (combateTerminado)
            return;

        StartCoroutine(SecuenciaConsumible(index));
    }

    void MostrarStats()
    {
        if (jugador == null || enemigo == null)
            return;

        EnviarMensaje(jugador.ObtenerStats());
        EnviarMensaje(enemigo.ObtenerStats());
    }

    void OnDestroy()
    {
        if (corrutinavolverAlMapa != null)
        {
            StopCoroutine(corrutinavolverAlMapa);
        }
    }

    #endregion

    #region === GETTERS (para UI y otros sistemas) ===

    public CombatState ObtenerEstado() => stateMachine.CurrentState;

    public bool PuedeJugadorActuar() => stateMachine.CanPlayerAct();

    public bool EstaAccionEjecutandose() => stateMachine.IsActionExecuting();

    #endregion
}