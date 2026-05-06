using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instancia;
    private AudioSource audioSource;
    private AudioClip musicaActual;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void CambiarMusica(AudioClip musica)
    {
        if (audioSource.clip == musica) return;
        audioSource.clip = musica;
        musicaActual = musica;
        audioSource.Play();
    }

    public void DetenerMusica()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    public void ReanudarMusica(AudioClip musica)
    {
        if (musica != null)
        {
            CambiarMusica(musica);
        }
    }
}