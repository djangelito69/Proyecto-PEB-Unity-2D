using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Escenas")]
    [SerializeField] private string escenaSeleccionPersonaje;

    public void NuevaPartida()
    {
        if (PartidaManager.instancia != null)
        {
            PartidaManager.instancia.NuevaPartida();
        }

        SceneManager.LoadScene(escenaSeleccionPersonaje);
    }

    public void CargarPartida()
    {
        Debug.Log("Pantalla de cargar partida");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}