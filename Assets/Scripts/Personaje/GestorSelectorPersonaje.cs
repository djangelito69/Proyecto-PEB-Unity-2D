using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorSelectorPersonaje : MonoBehaviour
{

    private DatosPersonaje.TipoPersonaje seleccionActual = DatosPersonaje.TipoPersonaje.Gato;


    public void SeleccionarGato()
    {
        seleccionActual = DatosPersonaje.TipoPersonaje.Gato;
        Debug.Log("Seleccionado: Gato");
    }

    public void SeleccionarPerro()
    {
        seleccionActual = DatosPersonaje.TipoPersonaje.Perro;
        Debug.Log("Seleccionado: Perro");
    }

    public void SeleccionarRaton()
    {
        seleccionActual = DatosPersonaje.TipoPersonaje.Raton;
        Debug.Log("Seleccionado: Ratón");
    }

    public void Confirmar()
    {
        DatosPersonaje.ElegirPersonaje(seleccionActual);

        if (GestorExperiencia.instancia != null)
        {
            GestorExperiencia.instancia.ReiniciarDatos();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Volver()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}