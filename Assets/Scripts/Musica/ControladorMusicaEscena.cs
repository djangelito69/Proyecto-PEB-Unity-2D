// ControladorMusicaEscena.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMusicaEscena : MonoBehaviour
{
    [SerializeField] private AudioClip musicaEscena;

    private void OnEnable()
    {
        ReproducirMiMusica();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != gameObject.scene.name) return;
        ReproducirMiMusica();
    }

    public void ReproducirMiMusica()
    {
        if (MusicManager.instancia == null) return;

        if (musicaEscena == null)
        {
            MusicManager.instancia.DetenerMusica();
            return;
        }

        // ✅ Verificar AQUÍ antes de llamar al manager
        // Si ya está sonando el mismo clip, no hacer nada
        if (MusicManager.instancia.EstaReproduciendo(musicaEscena)) return;

        MusicManager.instancia.ReproducirMusica(musicaEscena);
    }
}