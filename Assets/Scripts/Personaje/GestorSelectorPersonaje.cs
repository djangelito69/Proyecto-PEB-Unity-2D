using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorSelectorPersonaje : MonoBehaviour
{
    public string nombreEscenaCombate = "Combate";

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
        Debug.Log($"Confirmado: {seleccionActual} → cargando {nombreEscenaCombate}");
        SceneManager.LoadScene(nombreEscenaCombate);
    }

    public void Volver()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}