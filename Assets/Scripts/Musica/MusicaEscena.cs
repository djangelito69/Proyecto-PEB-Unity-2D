using UnityEngine;

public class MusicaEscena : MonoBehaviour
{
    public AudioClip musica;

    void Start()
    {
        MusicManager.instancia.CambiarMusica(musica);
    }
}