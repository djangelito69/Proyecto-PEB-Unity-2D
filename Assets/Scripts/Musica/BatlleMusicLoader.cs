using UnityEngine;

public class BattleMusicLoader : MonoBehaviour
{
    [SerializeField] private AudioClip musicaCombate;

    void Start()
    {
        if (MusicManager.instancia != null && musicaCombate != null)
        {
            MusicManager.instancia.CambiarMusica(musicaCombate);
        }
    }
}