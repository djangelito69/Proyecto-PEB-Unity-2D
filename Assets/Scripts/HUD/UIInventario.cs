using TMPro;
using UnityEngine;

public class UIInventario : MonoBehaviour
{
    public TextMeshProUGUI textoInventario;

    void OnEnable()
    {
        ActualizarInventario();
    }

    public void ActualizarInventario()
    {
        textoInventario.text = "";

        var items = Inventario.instancia.ObtenerItems();

        if (items.Count == 0)
        {
            textoInventario.text = "Inventario vacío";
        }

        foreach (var item in items)
        {
            textoInventario.text +=
                item.Key + " x" + item.Value + "\n";
        }
    }
}