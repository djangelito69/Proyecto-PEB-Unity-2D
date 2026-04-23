using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISelectorPersonaje : MonoBehaviour
{
    [Header("Referencia al manager")]
    public GestorSelectorPersonaje gestorSelector;

    [Header("Botones de personaje")]
    public Button botonGato;
    public Button botonPerro;
    public Button botonRaton;

    [Header("Colores de selección")]
    public Color colorSeleccionado = Color.yellow;
    public Color colorNormal = Color.white;

    [Header("Panel de stats")]
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoStats;

    private DatosPersonaje.TipoPersonaje seleccionActual = DatosPersonaje.TipoPersonaje.Gato;

    void Start()
    {
        botonGato.onClick.AddListener(() => Seleccionar(DatosPersonaje.TipoPersonaje.Gato));
        botonPerro.onClick.AddListener(() => Seleccionar(DatosPersonaje.TipoPersonaje.Perro));
        botonRaton.onClick.AddListener(() => Seleccionar(DatosPersonaje.TipoPersonaje.Raton));

        // Mostrar el Gato seleccionado por defecto
        Seleccionar(DatosPersonaje.TipoPersonaje.Gato);
    }

    void Seleccionar(DatosPersonaje.TipoPersonaje tipo)
    {
        seleccionActual = tipo;

        // Avisar al manager
        switch (tipo)
        {
            case DatosPersonaje.TipoPersonaje.Gato: gestorSelector.SeleccionarGato(); break;
            case DatosPersonaje.TipoPersonaje.Perro: gestorSelector.SeleccionarPerro(); break;
            case DatosPersonaje.TipoPersonaje.Raton: gestorSelector.SeleccionarRaton(); break;
        }

        ActualizarBotones();
        MostrarStats(tipo);
    }

    void ActualizarBotones()
    {
        botonGato.image.color = seleccionActual == DatosPersonaje.TipoPersonaje.Gato ? colorSeleccionado : colorNormal;
        botonPerro.image.color = seleccionActual == DatosPersonaje.TipoPersonaje.Perro ? colorSeleccionado : colorNormal;
        botonRaton.image.color = seleccionActual == DatosPersonaje.TipoPersonaje.Raton ? colorSeleccionado : colorNormal;
    }

    void MostrarStats(DatosPersonaje.TipoPersonaje tipo)
    {
        switch (tipo)
        {
            case DatosPersonaje.TipoPersonaje.Gato:
                textoNombre.text = "Gato";
                textoStats.text = "Vida: 15   PA: 10\nAtaque básico: 3 (costo 2)\nAtaque especial: 6 (costo 5)";
                break;
            case DatosPersonaje.TipoPersonaje.Perro:
                textoNombre.text = "Perro";
                textoStats.text = "Vida: 20   PA: 7\nAtaque básico: 4 (costo 2)\nAtaque especial: 7 (costo 5)";
                break;
            case DatosPersonaje.TipoPersonaje.Raton:
                textoNombre.text = "Ratón";
                textoStats.text = "Vida: 10   PA: 15\nAtaque básico: 2 (costo 1)\nAtaque especial: 8 (costo 4)";
                break;
        }
    }
}