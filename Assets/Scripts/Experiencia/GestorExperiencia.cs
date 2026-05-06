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
    private DatosCombate statsBase;
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

        if (statsBase == null)
        {
            Debug.LogError($"[GestorExperiencia] ❌ ERROR: No se puede obtener datos del personaje '{DatosPersonaje.PersonajeSeleccionado}'.\n" +
                          "Asegúrate de que DatosPersonaje contiene este personaje.");
            enabled = false;
            return;
        }

        paActualJugador = statsBase.pa;
        vidaActualJugador = statsBase.vida;

        // Calcular XP necesario para siguiente nivel
        CalcularExperienciaParaSiguiente();

        // Notificar UI
        OnExperienciaActualizada?.Invoke(nivelActual, experienciaActual, experienciaParaProximo);
        OnVidaActualizada?.Invoke();

        inicializado = true;

        Debug.Log($"[GestorExperiencia] ✓ Inicializado correctamente.\n" +
                 $"Personaje: {statsBase.nombre} | Nivel: {nivelActual} | Vida: {vidaActualJugador}/{statsBase.vida}");
    }
    public int ObtenerPAActual() => paActualJugador;

    public void EstablecerPAActual(int pa)
    {
        if (statsBase == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se puede establecer PA: statsBase es null.");
            return;
        }

        paActualJugador = Mathf.Clamp(pa, 0, statsBase.pa);
        OnVidaActualizada?.Invoke(); // si luego haces evento de PA, mejor separarlo
        Debug.Log($"[PA] Actualizado a: {paActualJugador}/{statsBase.pa}");
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
        while (experienciaActual >= experienciaParaProximo)
        {
            SubirDeNivel();
        }

        OnExperienciaActualizada?.Invoke(nivelActual, experienciaActual, experienciaParaProximo);
    }

    void SubirDeNivel()
    {
        experienciaActual -= experienciaParaProximo;
        nivelActual++;

        // Aplicar mejoras de stats
        statsBase.vida += vidaPorNivel;
        statsBase.dañoBasico += dañoPorNivel;
        statsBase.dañoEspecial += (int)(dañoPorNivel * 1.5f);
        statsBase.recuperacionPA += recoveryPAPorNivel;

        // Restaurar vida si está habilitado
        if (restaurarVidaAlSubir)
        {
            vidaActualJugador = statsBase.vida;
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
        experienciaParaProximo = (int)(experienciaBase * Mathf.Pow(multiplicadorExperiencia, nivelActual - 1));
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
    public int ObtenerVidaMaxima() => statsBase != null ? statsBase.vida : 0;
    public float ObtenerPorcentajeVida()
    {
        if (statsBase == null || statsBase.vida == 0) return 0f;
        return (float)vidaActualJugador / statsBase.vida;
    }

    public int ObtenerDañoBasico() => statsBase != null ? statsBase.dañoBasico : 0;
    public int ObtenerDañoEspecial() => statsBase != null ? statsBase.dañoEspecial : 0;
    public int ObtenerRecuperacionPA() => statsBase != null ? statsBase.recuperacionPA : 0;

    // ========== SETTERS ==========

    /// <summary>
    /// Establece la vida actual del jugador (después de un combate)
    /// </summary>
    public void EstablecerVidaActual(int vida)
    {
        if (statsBase == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se puede establecer vida: statsBase es null.");
            return;
        }

        vidaActualJugador = Mathf.Clamp(vida, 0, statsBase.vida);
        OnVidaActualizada?.Invoke();
        Debug.Log($"[VIDA] Actualizada a: {vidaActualJugador}/{statsBase.vida}");
    }

    /// <summary>
    /// Obtener un DatosCombate actualizado con los stats del nivel actual
    /// </summary>
    public DatosCombate ObtenerDatosActuales()
    {
        if (statsBase == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se pueden obtener datos actuales: statsBase es null.");
            return null;
        }

        return new DatosCombate
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
        if (statsBase == null)
        {
            Debug.LogError("[GestorExperiencia] ❌ No se pueden obtener datos para guardar: statsBase es null.");
            return null;
        }

        return new DatosGuardados
        {
            nivel = nivelActual,
            experienciaActual = experienciaActual,
            vidaActual = vidaActualJugador,
            vidaMaxima = statsBase.vida,
            dañoBasico = statsBase.dañoBasico,
            dañoEspecial = statsBase.dañoEspecial
        };
    }
}