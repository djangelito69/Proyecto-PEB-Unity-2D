using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instancia;

    private AudioSource audioSource;

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
        audioSource.Play();
    }
}