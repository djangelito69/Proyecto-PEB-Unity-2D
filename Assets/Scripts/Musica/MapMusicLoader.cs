using UnityEngine;

public class MapMusicLoader : MonoBehaviour
{
    [SerializeField] private AudioClip musicaMapa;

    void Start()
    {
        if (MusicManager.instancia != null && musicaMapa != null)
        {
            MusicManager.instancia.CambiarMusica(musicaMapa);
        }
    }
}