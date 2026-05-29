using UnityEngine;
using UnityEngine.UI;

public class UIRetratoPersonaje : MonoBehaviour
{
    [Header("Imagen UI")]
    public Image imagenPersonaje;

    [Header("Sprites")]
    public Sprite gato;
    public Sprite perro;
    public Sprite raton;

    void Start()
    {
        CambiarRetrato();
    }

    void CambiarRetrato()
    {
        switch (DatosPersonaje.PersonajeSeleccionado)
        {
            case DatosPersonaje.TipoPersonaje.Gato:
                imagenPersonaje.sprite = gato;
                break;

            case DatosPersonaje.TipoPersonaje.Perro:
                imagenPersonaje.sprite = perro;
                break;

            case DatosPersonaje.TipoPersonaje.Raton:
                imagenPersonaje.sprite = raton;
                break;
        }
    }
}