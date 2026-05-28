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

    public void ReproducirMusica(AudioClip nuevaMusica)
    {
        if (nuevaMusica == null)
        {
            DetenerMusica();
            return;
        }

        if (audioSource.clip == nuevaMusica &&
            audioSource.isPlaying)
        {
            return;
        }

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