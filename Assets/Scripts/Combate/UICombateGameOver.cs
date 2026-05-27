using System.Collections; // Añadido para poder usar IEnumerator
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la pantalla de GameOver cuando el jugador pierde el combate.
/// Se muestra cuando la vida del jugador llega a 0.
/// </summary>
public class UICombateGameOver : MonoBehaviour
{
    [Header("Panel GameOver")]
    [SerializeField] private GameObject panelGameOver;

    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI textoTitulo;
    [SerializeField] private TextMeshProUGUI textoMensaje;
    [SerializeField] private TextMeshProUGUI textoNivel;
    [SerializeField] private TextMeshProUGUI textoVida;

    [Header("Botones")]
    [SerializeField] private Button botonReintentar;
    [SerializeField] private Button botonMenuPrincipal;

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoDerrota;
    [SerializeField] private AudioSource audioSource;

    [Header("Timing")]
    [SerializeField] private float tiempoMuestraGameOver = 2f;

    private GestorDeCombate gestorDeCombate;
    private bool gameOverMostrado = false;

    void Start()
    {
        // Obtener referencia al gestor de combate
        gestorDeCombate = GestorDeCombate.instancia;

        // Asegurar que el panel de GameOver está oculto al inicio
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }

        // Conectar botones
        if (botonReintentar != null)
        {
            botonReintentar.onClick.AddListener(Reintentar);
        }

        if (botonMenuPrincipal != null)
        {
            botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);
        }

        // Suscribirse a cambios en el combate
        if (gestorDeCombate != null)
        {
            gestorDeCombate.OnMensajeCombate += VerificarDerrota;
        }
    }

    void OnDestroy()
    {
        // ? CRÍTICO: Desuscribirse y remover listeners para evitar memory leaks
        if (gestorDeCombate != null)
        {
            gestorDeCombate.OnMensajeCombate -= VerificarDerrota;
        }

        if (botonReintentar != null)
        {
            botonReintentar.onClick.RemoveListener(Reintentar);
        }

        if (botonMenuPrincipal != null)
        {
            botonMenuPrincipal.onClick.RemoveListener(IrAlMenuPrincipal);
        }

        Debug.Log("UICombateGameOver: Listeners removidos");
    }

    /// <summary>
    /// Verifica si el jugador ha perdido y muestra la pantalla de GameOver.
    /// Se llama cada vez que hay un mensaje de combate.
    /// </summary>
    private void VerificarDerrota(string mensaje)
    {
        if (gameOverMostrado) return;

        if (gestorDeCombate.combateTerminado && !gestorDeCombate.jugador.EstaVivo)
        {
            gameOverMostrado = true;

            // En lugar de mostrarlo instantáneamente, iniciamos la corrutina
            StartCoroutine(MostrarGameOverConRetraso());
        }
    }

    /// <summary>
    /// Espera el tiempo definido antes de mostrar el Game Over.
    /// </summary>
    private IEnumerator MostrarGameOverConRetraso()
    {
        // Usamos la variable de tiempo para crear un retraso
        yield return new WaitForSeconds(tiempoMuestraGameOver);

        // Una vez que pasa el tiempo, mostramos la pantalla
        MostrarGameOver();
    }

    /// <summary>
    /// Muestra la pantalla de GameOver con información del combate.
    /// </summary>
    private void MostrarGameOver()
    {
        Debug.Log("UICombateGameOver: Mostrando pantalla de derrota");

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        // Reproducir sonido de derrota
        if (audioSource != null && sonidoDerrota != null)
        {
            audioSource.PlayOneShot(sonidoDerrota);
        }

        // Actualizar textos
        if (textoTitulo != null)
        {
            textoTitulo.text = "¡PERDISTE!";
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = $"{gestorDeCombate.enemigo.Nombre} te ha derrotado...";
        }

        if (textoNivel != null && GestorExperiencia.instancia != null)
        {
            textoNivel.text = $"Nivel: {GestorExperiencia.instancia.ObtenerNivel()}";
        }

        if (textoVida != null)
        {
            textoVida.text = $"Vida: {gestorDeCombate.jugador.vidaActual}/{gestorDeCombate.jugador.vidaMaxima}";
        }

        // Pausar el juego mientras se muestra GameOver
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Reintentar el combate.
    /// </summary>
    public void Reintentar()
    {
        Time.timeScale = 1f;

        // RESET VIDA EN SISTEMA DE PROGRESO
        if (GestorExperiencia.instancia != null)
        {
            DatosCombate baseStats =
                DatosPersonaje.ObtenerDatos(DatosPersonaje.PersonajeSeleccionado);

            GestorExperiencia.instancia.EstablecerVidaActual(baseStats.vida);
            GestorExperiencia.instancia.EstablecerPAActual(baseStats.pa);
        }

        if (GestorCombateGlobal.instancia != null)
        {
            GestorCombateGlobal.instancia.ReestablecerTransicion();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Ir al menú principal.
    /// </summary>
    public void IrAlMenuPrincipal()
    {
        Debug.Log("UICombateGameOver: Ir a menú principal");

        Time.timeScale = 1f;

        // ? IMPORTANTE: Reestablecer el flag antes de ir a menú
        if (GestorCombateGlobal.instancia != null)
        {
            GestorCombateGlobal.instancia.ReestablecerTransicion();
            Debug.Log("UICombateGameOver: Flag de transición reestablecido");
        }

        // Ir al menú principal
        SceneManager.LoadScene("MenuPrincipal");
    }
}