using UnityEngine;
using UnityEngine.UI;

public class MusicaFondo : MonoBehaviour
{
    private AudioSource audioSource;

    public Button botonMusica;
    public Color colorPausado;

    private Color colorOriginal;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        colorOriginal = botonMusica.image.color;
    }

    public void ToggleMusica()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            botonMusica.image.color = colorPausado;
        }
        else
        {
            audioSource.UnPause();
            botonMusica.image.color = colorOriginal;
        }
    }
}