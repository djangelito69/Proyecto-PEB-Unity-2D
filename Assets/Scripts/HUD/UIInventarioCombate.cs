using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventarioCombate : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelInventario;

    [Header("Slots")]
    public Button[] botonesSlots;

    [Header("UI")]
    public Image[] iconos;
    public TMP_Text[] nombres;
    public TMP_Text[] cantidades;

    private int indiceSeleccionado = -1;

    public void AbrirInventario()
    {
        if (GestorDeCombate.instancia == null)
            return;

        if (!GestorDeCombate.instancia.PuedeJugadorActuar())
            return;

        panelInventario.SetActive(true);

        ActualizarUI();
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);
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
                    "x" + lista[i].cantidad;

                iconos[i].sprite =
                    lista[i].icono;

                int index = i;

                botonesSlots[i].onClick.RemoveAllListeners();

                botonesSlots[i].onClick.AddListener(() =>
                {
                    indiceSeleccionado = index;
                });
            }
            else
            {
                botonesSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void BotonUsar()
    {
        if (indiceSeleccionado == -1)
            return;

        GestorDeCombate.instancia
            .UsarConsumibleCombate(indiceSeleccionado);

        panelInventario.SetActive(false);
    }

    public void BotonCerrar()
    {
        panelInventario.SetActive(false);
    }
}