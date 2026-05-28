using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMusicaEscena : MonoBehaviour
{
    [SerializeField] private AudioClip musicaEscena;

    private void OnEnable()  // <-- cambia Start por OnEnable
    {
        ReproducirMiMusica();
        SceneManager.sceneLoaded += OnSceneLoaded; // por si acaso
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cuando se descarga combate y el mapa queda activo
        if (scene.name != gameObject.scene.name) return;
        ReproducirMiMusica();
    }

    public void ReproducirMiMusica()
    {
        Debug.Log($"[MUSICA] ReproducirMiMusica llamado en escena: {gameObject.scene.name} | clip: {(musicaEscena != null ? musicaEscena.name : "NULL")}");

        if (MusicManager.instancia == null) return;

        if (musicaEscena == null)
            MusicManager.instancia.DetenerMusica();
        else
            MusicManager.instancia.ReproducirMusica(musicaEscena);
    }
}