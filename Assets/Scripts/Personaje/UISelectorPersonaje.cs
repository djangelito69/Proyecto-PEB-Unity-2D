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
        DatosCombate datos = DatosPersonaje.ObtenerDatos(tipo);

        textoNombre.text = datos.nombre;

        textoStats.text =
            $"Vida: {datos.vida}   PA: {datos.pa}\n" +
            $"Ataque básico: {datos.dañoBasico} (costo {datos.costoBasico})\n" +
            $"Ataque especial: {datos.dañoEspecial} (costo {datos.costoEspecial})";
    }
}