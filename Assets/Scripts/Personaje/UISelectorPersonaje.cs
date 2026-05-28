using UnityEngine;
using UnityEngine.UI;

public class UISelectorPersonaje : MonoBehaviour
{
    [Header("Manager")]
    public GestorSelectorPersonaje gestorSelector;

    [Header("Paneles")]
    public GameObject panelGato;
    public GameObject panelPerro;
    public GameObject panelRaton;

    [Header("Botones panel gato")]
    public Button botonGatoAPerro;
    public Button botonGatoARaton;

    [Header("Botones panel perro")]
    public Button botonPerroAGato;
    public Button botonPerroARaton;

    [Header("Botones panel raton")]
    public Button botonRatonAGato;
    public Button botonRatonAPerro;

    void Start()
    {
        // BOTONES GATO
        botonGatoAPerro.onClick.AddListener(() =>
        {
            MostrarPanelPerro();
        });

        botonGatoARaton.onClick.AddListener(() =>
        {
            MostrarPanelRaton();
        });

        // BOTONES PERRO
        botonPerroAGato.onClick.AddListener(() =>
        {
            MostrarPanelGato();
        });

        botonPerroARaton.onClick.AddListener(() =>
        {
            MostrarPanelRaton();
        });

        // BOTONES RATON
        botonRatonAGato.onClick.AddListener(() =>
        {
            MostrarPanelGato();
        });

        botonRatonAPerro.onClick.AddListener(() =>
        {
            MostrarPanelPerro();
        });

        // Panel inicial
        MostrarPanelGato();
    }

    public void MostrarPanelGato()
    {
        panelGato.SetActive(true);
        panelPerro.SetActive(false);
        panelRaton.SetActive(false);

        gestorSelector.SeleccionarGato();
    }

    public void MostrarPanelPerro()
    {
        panelGato.SetActive(false);
        panelPerro.SetActive(true);
        panelRaton.SetActive(false);

        gestorSelector.SeleccionarPerro();
    }

    public void MostrarPanelRaton()
    {
        panelGato.SetActive(false);
        panelPerro.SetActive(false);
        panelRaton.SetActive(true);

        gestorSelector.SeleccionarRaton();
    }
}