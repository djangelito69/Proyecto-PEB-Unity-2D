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

    [Header("Sonidos UI")]
    public AudioSource audioSource;

    public AudioClip sonidoClick;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;

    [Header("Botón Pausa")]
    public Image botonPausaImage; // Asignar el componente Image del botón en el Inspector
    public Sprite iconoPausar1;   // Sprite por defecto (cuando no está pausado)
    public Sprite iconoPausar2;   // Sprite cuando está pausado

    [Header("Botón Musica")]
    public Image botonMusicaImage; // Asignar el componente Image del botón en el Inspector
    public Sprite iconoMusica1;   // Sprite por defecto (cuando no está silenciada)
    public Sprite iconoMusica2;   // Sprite cuando está silenciada

    [Header("Mensaje de Pausa")]
    public GameObject textoPausa;

    private bool juegoPausado = false;
    private bool musicaSilenciada = false;

    private void Start()
    {
        // Garantiza el icono por defecto al iniciar
        if (botonPausaImage != null && iconoPausar1 != null)
        {
            botonPausaImage.sprite = iconoPausar1;
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
    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void TogglePausa()
    {
        ReproducirSonido(sonidoClick);

        Debug.Log("El botón de pausa fue presionado. Estado: " + !juegoPausado);

        juegoPausado = !juegoPausado;

        Time.timeScale = juegoPausado ? 0f : 1f;
        AudioListener.pause = juegoPausado;

        // Mostrar u ocultar mensaje
        if (textoPausa != null)
        {
            textoPausa.SetActive(juegoPausado);
        }

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

        if (botonPausaImage != null)
        {
            botonMusicaImage.sprite = musicaSilenciada
                ? (iconoMusica2 ?? botonMusicaImage.sprite)
                : (iconoMusica1 ?? botonMusicaImage.sprite);
        }
    }
    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);

        ReproducirSonido(sonidoCerrar);
    }

    public void ToggleOpciones()
    {
        bool abrir = !panelOpciones.activeSelf;

        panelOpciones.SetActive(abrir);

        ReproducirSonido(
            abrir ? sonidoAbrir : sonidoCerrar
        );
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);

        ReproducirSonido(sonidoCerrar);
    }

    public void IrAMenuPrincipal()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuPrincipal");
    }
}