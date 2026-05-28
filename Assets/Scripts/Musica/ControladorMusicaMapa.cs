using UnityEngine;

public class ControladorMusicaMapa : MonoBehaviour
{
    [SerializeField] private AudioClip musicaMapa;

    public AudioClip ObtenerMusicaMapa()
    {
        return musicaMapa;
    }
}