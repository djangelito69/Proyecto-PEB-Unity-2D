using UnityEngine;

public class Musica : MonoBehaviour
{

    public static Musica instancia;

    public AudioSource backgroundSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;

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
        }

    }


    public void ToggleMusica()
    {
        if (backgroundSource.isPlaying)
        {
            backgroundSource.Pause();
        }
        else
        {
            backgroundSource.UnPause();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void ToggleAmbiente()
    {
        if (ambientSource.isPlaying)
        {
            ambientSource.Pause();
        }
        else
        {
            ambientSource.UnPause();
        }
    }

}