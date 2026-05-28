using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestor global de transiciones de combate.
/// Este script es persistente y existe desde el inicio del juego.
/// Controla que solo un combate se inicie a la vez, incluso si varios enemigos colisionan.
/// Se sincroniza con GestorDeCombate cuando la escena de combate carga.
/// </summary>
public class GestorCombateGlobal : MonoBehaviour
{
    public static GestorCombateGlobal instancia { get; private set; }

    /// <summary>
    /// Flag que previene cargas múltiples de escenas si varios enemigos colisionan simultáneamente
    /// </summary>
    public bool combateEnTransicion { get; private set; } = false;

    /// <summary>
    /// Caché de referencias para optimización masiva (evita búsquedas en tiempo de ejecución)
    /// </summary>
    private GameObject jugadorCacheado;
    private PlayerImmunity playerImmunityCacheada;
    private AudioListener audioListenerPrincipalCacheado;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GestorCombateGlobal: Inicializado como singleton persistente");

        // Suscribirse a eventos de escena
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // Optimización: Cachear referencias al inicio
        CachearReferencias();
    }

    void OnDestroy()
    {
        // Desuscribirse al destruir
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    /// <summary>
    /// Cachea las referencias del entorno principal.
    /// </summary>
    private void CachearReferencias()
    {
        // Cachear Jugador
        if (jugadorCacheado == null)
        {
            jugadorCacheado = GameObject.FindGameObjectWithTag("Player");
            if (jugadorCacheado != null)
            {
                playerImmunityCacheada = jugadorCacheado.GetComponent<PlayerImmunity>();
                Debug.Log("GestorCombateGlobal: Referencia del jugador cacheada");
            }
            else
            {
                Debug.LogWarning("GestorCombateGlobal: No se encontró GameObject con tag 'Player'");
            }
        }

        // Cachear el AudioListener principal del mapa (Reemplazo moderno para FindObjectOfType)
        if (audioListenerPrincipalCacheado == null)
        {
            audioListenerPrincipalCacheado = Object.FindFirstObjectByType<AudioListener>();
            if (audioListenerPrincipalCacheado != null)
            {
                Debug.Log($"GestorCombateGlobal: AudioListener principal cacheado: {audioListenerPrincipalCacheado.gameObject.name}");
            }
        }
    }

    /// <summary>
    /// Se llama cuando una escena se descarga.
    /// Reestablece el flag si la escena de combate fue descargada.
    /// </summary>
    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "Combate")
        {
            Debug.Log("GestorCombateGlobal: Escena de combate descargada. Reestableciendo transición automáticamente.");
            ReestablecerTransicion();
        }
    }

    /// <summary>
    /// Intenta iniciar un combate. Solo el primer enemigo que llama a este método puede iniciar.
    /// </summary>
    public bool IntentarIniciarCombate(DatosEnemigos.TipoEnemigo tipoEnemigo, DatosCombate datosCombate, GameObject enemigoGameObject)
    {
        // Si ya hay un combate en transición, rechazar esta solicitud
        if (combateEnTransicion)
        {
            Debug.Log($"GestorCombateGlobal: Combate ya en transición. Enemigo {datosCombate.nombre} ignorado.");
            return false;
        }

        // Seguridad: Si cambiamos de mapa y las referencias se perdieron, volvemos a buscarlas
        if (jugadorCacheado == null || audioListenerPrincipalCacheado == null)
        {
            CachearReferencias();
        }

        Debug.Log($"GestorCombateGlobal: Iniciando combate con {datosCombate.nombre}");
        combateEnTransicion = true;

        // Registrar el enemigo en GestorEnemigos
        GestorEnemigos.instancia.enemigoEnMapa = enemigoGameObject;
        GestorEnemigos.instancia.EstablecerEnemigo(tipoEnemigo, datosCombate);


        // Desactivar el AudioListener principal de forma directa y limpia
        AlternarAudioListenerPrincipal(false);

        // Cargar escena de combate de forma aditiva
        SceneManager.LoadScene("Combate", LoadSceneMode.Additive);

        return true;
    }

    /// <summary>
    /// Restablece el flag de transición cuando se vuelve del combate.
    /// Debe ser llamado al salir de la escena de combate.
    /// </summary>
    public void ReestablecerTransicion()
    {
        combateEnTransicion = false;

        AlternarAudioListenerPrincipal(true);

        if (playerImmunityCacheada != null)
        {
            playerImmunityCacheada.ActivarInmunidad();
        }

        // Restaurar música del mapa
        ControladorMusicaEscena controlador = FindFirstObjectByType<ControladorMusicaEscena>();
        if (controlador != null)
        {
            Debug.Log($"[MUSICA] Controlador encontrado en: {controlador.gameObject.scene.name}");
            controlador.ReproducirMiMusica();
        }
        else
        {
            Debug.Log("[MUSICA] No se encontró ControladorMusicaEscena");
        }

        Debug.Log("GestorCombateGlobal: Transición reestablecida. Listo para próximo combate.");
    }

    /// <summary>
    /// Enciende o apaga el AudioListener del mapa principal de manera segura.
    /// </summary>
    private void AlternarAudioListenerPrincipal(bool estado)
    {
        if (audioListenerPrincipalCacheado != null)
        {
            audioListenerPrincipalCacheado.enabled = estado;
            Debug.Log($"GestorCombateGlobal: AudioListener principal {(estado ? "ACTIVADO" : "DESACTIVADO")}");
        }
        else
        {
            // Failsafe por si cambió de escena/mapa y el anterior se destruyó
            audioListenerPrincipalCacheado = Object.FindFirstObjectByType<AudioListener>();
            if (audioListenerPrincipalCacheado != null)
            {
                audioListenerPrincipalCacheado.enabled = estado;
            }
        }
    }
}