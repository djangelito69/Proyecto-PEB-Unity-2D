using UnityEngine;
using System;

/// <summary>
/// Sistema de experiencia y niveles del jugador.
/// Se integra con DatosPersonaje y mantiene la vida persistente entre combates.
/// </summary>
public class GestorExperiencia : MonoBehaviour
{
    public static GestorExperiencia instancia { get; private set; }

    [Header("Configuración de Experiencia")]
    [SerializeField] private int experienciaBase = 50;        // XP para nivel 1→2
    [SerializeField] private float multiplicadorExperiencia = 1.5f;  // Crecimiento exponencial
    [SerializeField] private bool restaurarVidaAlSubir = true; // Restaurar vida al subir nivel

    [Header("Mejoras por Nivel")]
    [SerializeField] private int vidaPorNivel = 5;
    [SerializeField] private int dañoPorNivel = 1;
    [SerializeField] private int recoveryPAPorNivel = 1;

    // Estado del jugador
    private int nivelActual = 1;
    private int experienciaActual = 0;
    private int experienciaParaProximo = 0;
    private int paActualJugador;

    // Stats persistentes
    private DatosCombate statsBase;      // Datos originales del personaje
    private DatosCombate statsRuntime;   // Datos que cambian con los niveles
    private int vidaActualJugador;
    private bool inicializado = false;

    // Eventos
    public event Action<int, int, int> OnExperienciaActualizada;  // (nivel, exp actual, exp próximo)
    public event Action<int> OnNivelSubido;                        // (nuevo nivel)
    public event Action OnVidaActualizada;                         // Cuando la vida cambia

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Debug.LogWarning("[GestorExperiencia] ⚠ Ya existe una instancia. Destruyendo duplicado.");
            Destroy(gameObject);
            return;
        }

        instancia = this;

        // Desvincular del padre para convertirlo en un objeto raíz
        transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
        Debug.Log("[GestorExperiencia] Instancia singleton creada en Awake.");
    }

    void Start()
    {
        Debug.Log("[GestorExperiencia] Iniciando Start()...");

        // Validar que DatosPersonaje esté disponible
        if (!Enum.IsDefined(typeof(DatosPersonaje.TipoPersonaje), DatosPersonaje.PersonajeSeleccionado))
        {
            Debug.LogError("[GestorExperiencia] ❌ ERROR: No hay personaje seleccionado en DatosPersonaje.PersonajeSeleccionado");
            enabled = false;
            return;
        }

        // Obtener datos base del personaje seleccionado
        statsBase = DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);

        // Crear copia independiente (runtime) para modificar con los niveles
        statsRuntime = new DatosCombate
        {
            nombre = statsBase.nombre,
            vida = statsBase.vida,
            pa = statsBase.pa,
            dañoBasico = statsBase.dañoBasico,
            dañoEspecial = statsBase.dañoEspecial,
            costoBasico = statsBase.costoBasico,
            costoEspecial = statsBase.costoEspecial,
            recuperacionPA = statsBase.recuperacionPA,
            sprite = statsBase.sprite
        };

        // Usar statsRuntime para inicializar valores actuales
        paActualJugador = statsRuntime.pa;
        vidaActualJugador = statsRuntime.vida;

        // Calcular XP necesario para siguiente nivel
        CalcularExperienciaParaSiguiente();

        // Notificar UI
        OnExperienciaActualizada?.Invoke(nivelActual, experienciaActual, experienciaParaProximo);
        OnVidaActualizada?.Invoke();

        inicializado = true;

        Debug.Log($"[GestorExperiencia] ✓ Inicializado correctamente.\n" +
                 $"Personaje: {statsRuntime.nombre} | Nivel: {nivelActual} | Vida: {vidaActualJugador}/{statsRuntime.vida}");
    }

    public int ObtenerPAActual() => paActualJugador;

    public void EstablecerPAActual(int pa)
    {
        if (statsRuntime == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se puede establecer PA: statsRuntime es null.");
            return;
        }

        paActualJugador = Mathf.Clamp(pa, 0, statsRuntime.pa);
        OnVidaActualizada?.Invoke(); // si luego haces evento de PA, mejor separarlo
        Debug.Log($"[PA] Actualizado a: {paActualJugador}/{statsRuntime.pa}");
    }

    /// <summary>
    /// Verifica si el gestor está completamente inicializado
    /// </summary>
    public bool EstaInicializado() => inicializado;

    /// <summary>
    /// Añade experiencia y verifica subida de nivel
    /// </summary>
    public void AñadirExperiencia(int cantidad)
    {
        if (!inicializado)
        {
            Debug.LogWarning("[GestorExperiencia] ⚠ No se puede añadir experiencia: GestorExperiencia no está inicializado.");
            return;
        }

        experienciaActual += cantidad;
        Debug.Log($"[XP] +{cantidad} XP. Total: {experienciaActual}/{experienciaParaProximo}");

        // Verificar múltiples subidas de nivel en un golpe
        int controlBucle = 0;
        while (experienciaActual >= experienciaParaProximo)
        {
            if (experienciaParaProximo <= 0)
            {
                Debug.LogError("[GestorExperiencia] ❌ ERROR: experienciaParaProximo es <= 0. Se abortó el bucle para evitar congelamiento de Unity.");
                break;
            }

            controlBucle++;
            if (controlBucle > 1000)
            {
                Debug.LogError("[GestorExperiencia] ❌ ERROR: Bucle infinito detectado en AñadirExperiencia (más de 1000 iteraciones consecutivas).");
                break;
            }

            SubirDeNivel();
        }

        OnExperienciaActualizada?.Invoke(nivelActual, experienciaActual, experienciaParaProximo);
    }

    void SubirDeNivel()
    {
        experienciaActual -= experienciaParaProximo;
        nivelActual++;

        // Aplicar mejoras de stats a statsRuntime
        statsRuntime.vida += vidaPorNivel;
        statsRuntime.dañoBasico += dañoPorNivel;
        statsRuntime.dañoEspecial += (int)(dañoPorNivel * 1.5f);
        statsRuntime.recuperacionPA += recoveryPAPorNivel;

        // Restaurar vida si está habilitado
        if (restaurarVidaAlSubir)
        {
            vidaActualJugador = statsRuntime.vida;
            OnVidaActualizada?.Invoke();
        }

        // Calcular XP para siguiente nivel
        CalcularExperienciaParaSiguiente();

        Debug.Log($"[NIVEL UP] ¡Nivel {nivelActual}! Vida +{vidaPorNivel} | Daño +{dañoPorNivel}");
        OnNivelSubido?.Invoke(nivelActual);
    }

    void CalcularExperienciaParaSiguiente()
    {
        // Fórmula: expBase × multiplicador^(nivel-1)
        // Asegurar que los valores no sean cero o negativos en el Inspector por error
        int expBaseSegura = Mathf.Max(1, experienciaBase);
        float multSeguro = multiplicadorExperiencia <= 0f ? 1.5f : multiplicadorExperiencia;

        int calculo = (int)(expBaseSegura * Mathf.Pow(multSeguro, nivelActual - 1));
        experienciaParaProximo = Mathf.Max(1, calculo);
    }

    // ========== GETTERS ==========

    public int ObtenerNivel() => nivelActual;
    public int ObtenerExperienciaActual() => experienciaActual;
    public int ObtenerExperienciaParaProximo() => experienciaParaProximo;
    public float ObtenerPorcentajeExperiencia()
    {
        if (experienciaParaProximo == 0) return 0f;
        return (float)experienciaActual / experienciaParaProximo;
    }

    public int ObtenerVidaActual() => vidaActualJugador;
    public int ObtenerVidaMaxima() => statsRuntime != null ? statsRuntime.vida : 0;
    public float ObtenerPorcentajeVida()
    {
        if (statsRuntime == null || statsRuntime.vida == 0) return 0f;
        return (float)vidaActualJugador / statsRuntime.vida;
    }

    public int ObtenerDañoBasico() => statsRuntime != null ? statsRuntime.dañoBasico : 0;
    public int ObtenerDañoEspecial() => statsRuntime != null ? statsRuntime.dañoEspecial : 0;
    public int ObtenerRecuperacionPA() => statsRuntime != null ? statsRuntime.recuperacionPA : 0;

    // ========== SETTERS ==========

    /// <summary>
    /// Establece la vida actual del jugador (después de un combate)
    /// </summary>
    public void EstablecerVidaActual(int vida)
    {
        if (statsRuntime == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se puede establecer vida: statsRuntime es null.");
            return;
        }

        vidaActualJugador = Mathf.Clamp(vida, 0, statsRuntime.vida);
        OnVidaActualizada?.Invoke();
        Debug.Log($"[VIDA] Actualizada a: {vidaActualJugador}/{statsRuntime.vida}");
    }

    public void AñadirVidaMaxima(int cantidad)
    {
        statsRuntime.vida += cantidad;
        vidaActualJugador += cantidad;
        OnVidaActualizada?.Invoke();
    }

    public void AñadirPAMaximo(int cantidad)
    {
        statsRuntime.pa += cantidad;
        paActualJugador += cantidad;
        OnVidaActualizada?.Invoke();
    }

    public void AñadirDaño(int cantidad)
    {
        statsRuntime.dañoBasico += cantidad;
        statsRuntime.dañoEspecial += cantidad;
    }

    /// <summary>
    /// Obtener un DatosCombate actualizado con los stats del nivel actual
    /// </summary>
    public DatosCombate ObtenerDatosActuales()
    {
        if (statsRuntime == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se pueden obtener datos actuales: statsRuntime es null.");
            return null;
        }

        return new DatosCombate
        {
            nombre = statsRuntime.nombre,
            vida = statsRuntime.vida,
            pa = statsRuntime.pa,
            dañoBasico = statsRuntime.dañoBasico,
            dañoEspecial = statsRuntime.dañoEspecial,
            costoBasico = statsRuntime.costoBasico,
            costoEspecial = statsRuntime.costoEspecial,
            recuperacionPA = statsRuntime.recuperacionPA,
            sprite = statsRuntime.sprite
        };
    }

    public void InicializarPersonaje()
    {
        statsBase = DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);

        statsRuntime = new DatosCombate
        {
            nombre = statsBase.nombre,
            vida = statsBase.vida,
            pa = statsBase.pa,
            dañoBasico = statsBase.dañoBasico,
            dañoEspecial = statsBase.dañoEspecial,
            costoBasico = statsBase.costoBasico,
            costoEspecial = statsBase.costoEspecial,
            recuperacionPA = statsBase.recuperacionPA,
            sprite = statsBase.sprite
        };

        nivelActual = 1;
        experienciaActual = 0;
        experienciaParaProximo = 0;

        vidaActualJugador = statsRuntime.vida;
        paActualJugador = statsRuntime.pa;

        inicializado = true;

        CalcularExperienciaParaSiguiente();
    }

    public void ReiniciarDatos()
    {
        statsBase = DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);

        statsRuntime = new DatosCombate
        {
            nombre = statsBase.nombre,
            vida = statsBase.vida,
            pa = statsBase.pa,
            dañoBasico = statsBase.dañoBasico,
            dañoEspecial = statsBase.dañoEspecial,
            costoBasico = statsBase.costoBasico,
            costoEspecial = statsBase.costoEspecial,
            recuperacionPA = statsBase.recuperacionPA,
            sprite = statsBase.sprite
        };

        nivelActual = 1;
        experienciaActual = 0;
        experienciaParaProximo = 0;

        vidaActualJugador = statsRuntime.vida;
        paActualJugador = statsRuntime.pa;

        inicializado = true; // <- CRÍTICO

        CalcularExperienciaParaSiguiente();

        OnExperienciaActualizada?.Invoke(nivelActual, experienciaActual, experienciaParaProximo);
        OnVidaActualizada?.Invoke();
    }

    // ========== SAVE/LOAD (Futuro) ==========

    [System.Serializable]
    public class DatosGuardados
    {
        public int nivel;
        public int experienciaActual;
        public int vidaActual;
        public int vidaMaxima;
        public int dañoBasico;
        public int dañoEspecial;
    }

    public DatosGuardados ObtenerDatosParaGuardar()
    {
        if (statsRuntime == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se pueden obtener datos para guardar: statsRuntime es null.");
            return null;
        }

        return new DatosGuardados
        {
            nivel = nivelActual,
            experienciaActual = experienciaActual,
            vidaActual = vidaActualJugador,
            vidaMaxima = statsRuntime.vida,
            dañoBasico = statsRuntime.dañoBasico,
            dañoEspecial = statsRuntime.dañoEspecial
        };
    }
}