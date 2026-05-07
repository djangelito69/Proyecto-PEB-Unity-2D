using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventario : MonoBehaviour
{
    public static UIInventario instancia;

    [Header("Paneles")]
    public GameObject panelInventario;
    public GameObject panelConsumibles;
    public GameObject panelObjetos;

    [Header("Slots")]
    public Button[] botonesSlots;

    [Header("UI Slot")]
    public Image[] iconos;
    public TMP_Text[] nombres;
    public TMP_Text[] cantidades;

    private int indiceSeleccionado = -1;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        panelInventario.SetActive(false);

        ActualizarUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            panelInventario.SetActive(
                !panelInventario.activeSelf
            );

            MostrarConsumibles();
        }
    }

    public void MostrarConsumibles()
    {
        panelConsumibles.SetActive(true);
        panelObjetos.SetActive(false);
    }

    public void MostrarObjetos()
    {
        panelConsumibles.SetActive(false);
        panelObjetos.SetActive(true);
    }

    public void ActualizarUI()
    {
        var lista = Inventario.instancia.consumibles;

        for (int i = 0; i < botonesSlots.Length; i++)
        {
            if (i < lista.Count)
            {
                botonesSlots[i].gameObject.SetActive(true);

                nombres[i].text = lista[i].nombre;

                cantidades[i].text =
                    "x" + lista[i].cantidad.ToString();

                iconos[i].sprite = lista[i].icono;

                int index = i;

                botonesSlots[i].onClick.RemoveAllListeners();

                botonesSlots[i].onClick.AddListener(() =>
                {
                    SeleccionarConsumible(index);
                });
            }
            else
            {
                botonesSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void SeleccionarConsumible(int index)
    {
        indiceSeleccionado = index;
    }

    public void BotonUsar()
    {
        if (indiceSeleccionado != -1)
        {
            Inventario.instancia.UsarConsumible(
                indiceSeleccionado
            );
        }
    }

    public void BotonTirar()
    {
        if (indiceSeleccionado != -1)
        {
            Inventario.instancia.TirarConsumible(
                indiceSeleccionado
            );
        }
    }
}