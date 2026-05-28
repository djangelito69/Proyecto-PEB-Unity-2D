using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelInventario;
    public GameObject panelOpciones;
    public GameObject panelTeclas;

    [Header("Sonidos UI")]
    public AudioSource audioSource;

    public AudioClip sonidoClick;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;

    [Header("Botón Pausa")]
    public Image botonPausaImage;
    public Sprite iconoPausar1;
    public Sprite iconoPausar2;

    [Header("Botón Música")]
    public Image botonMusicaImage;
    public Sprite iconoMusica1;
    public Sprite iconoMusica2;

    [Header("Mensaje de Pausa")]
    public GameObject textoPausa;

    private bool juegoPausado = false;
    private bool musicaSilenciada = false;

    private void Start()
    {
        // Icono pausa inicial
        if (botonPausaImage != null && iconoPausar1 != null)
        {
            botonPausaImage.sprite = iconoPausar1;
        }

        // Icono música inicial
        if (botonMusicaImage != null && iconoMusica1 != null)
        {
            botonMusicaImage.sprite = iconoMusica1;
        }

        // Ocultar panel teclas al iniciar
        if (panelTeclas != null)
        {
            panelTeclas.SetActive(false);
        }
    }

    private void Update()
    {
        // Abrir/Cerrar menú opciones
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleOpciones();
        }

        // Pausar/Reanudar juego
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePausa();
        }

        // Silenciar/Activar música
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusica();
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void ToggleInventario()
    {
        bool abrir = !panelInventario.activeSelf;

        panelInventario.SetActive(abrir);

        ReproducirSonido(
            abrir ? sonidoAbrir : sonidoCerrar
        );
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);

        ReproducirSonido(sonidoCerrar);
    }

    public void TogglePausa()
    {
        ReproducirSonido(sonidoClick);

        juegoPausado = !juegoPausado;

        Time.timeScale = juegoPausado ? 0f : 1f;

        // Mostrar texto pausa
        if (textoPausa != null)
        {
            textoPausa.SetActive(juegoPausado);
        }

        // Cambiar icono pausa
        if (botonPausaImage != null)
        {
            botonPausaImage.sprite = juegoPausado
                ? (iconoPausar2 ?? botonPausaImage.sprite)
                : (iconoPausar1 ?? botonPausaImage.sprite);
        }
    }

    public void ToggleMusica()
    {
        ReproducirSonido(sonidoClick);

        musicaSilenciada = !musicaSilenciada;

        AudioListener.volume =
            musicaSilenciada ? 0f : 1f;

        // Cambiar icono música
        if (botonMusicaImage != null)
        {
            botonMusicaImage.sprite = musicaSilenciada
                ? (iconoMusica2 ?? botonMusicaImage.sprite)
                : (iconoMusica1 ?? botonMusicaImage.sprite);
        }
    }

    public void ToggleOpciones()
    {
        bool abrir = !panelOpciones.activeSelf;

        panelOpciones.SetActive(abrir);

        juegoPausado = abrir;

        Time.timeScale = abrir ? 0f : 1f;

        // Mostrar mensaje pausa
        if (textoPausa != null)
        {
            textoPausa.SetActive(abrir);
        }

        // Cambiar icono pausa
        if (botonPausaImage != null)
        {
            botonPausaImage.sprite = abrir
                ? (iconoPausar2 ?? botonPausaImage.sprite)
                : (iconoPausar1 ?? botonPausaImage.sprite);
        }

        ReproducirSonido(
            abrir ? sonidoAbrir : sonidoCerrar
        );
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);

        juegoPausado = false;

        Time.timeScale = 1f;

        if (textoPausa != null)
        {
            textoPausa.SetActive(false);
        }

        if (botonPausaImage != null)
        {
            botonPausaImage.sprite = iconoPausar1;
        }

        ReproducirSonido(sonidoCerrar);
    }

    public void Reanudar()
    {
        panelOpciones.SetActive(false);

        juegoPausado = false;

        Time.timeScale = 1f;

        if (textoPausa != null)
        {
            textoPausa.SetActive(false);
        }

        if (botonPausaImage != null)
        {
            botonPausaImage.sprite = iconoPausar1;
        }

        ReproducirSonido(sonidoCerrar);
    }

    public void AbrirPanelTeclas()
    {
        panelOpciones.SetActive(false);

        panelTeclas.SetActive(true);

        ReproducirSonido(sonidoAbrir);
    }

    public void RegresarAOpciones()
    {
        panelTeclas.SetActive(false);

        panelOpciones.SetActive(true);

        ReproducirSonido(sonidoCerrar);
    }

    public void IrAMenuPrincipal()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuPrincipal");
    }
}