// MusicManager.cs
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instancia;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ✅ Método público para que el controlador pueda consultar el estado
    public bool EstaReproduciendo(AudioClip clip)
    {
        return audioSource.clip == clip && audioSource.isPlaying;
    }

    public void ReproducirMusica(AudioClip nuevaMusica)
    {
        if (nuevaMusica == null)
        {
            DetenerMusica();
            return;
        }

        // Esta guarda sigue siendo útil como segunda línea de defensa
        if (audioSource.clip == nuevaMusica && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = nuevaMusica;
        audioSource.Play();
    }

    public void DetenerMusica()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    public void ReiniciarDatos()
    {
        DetenerMusica();
    }
}