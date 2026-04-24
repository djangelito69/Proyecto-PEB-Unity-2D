using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario instancia;

    private Dictionary<string, int> items =
        new Dictionary<string, int>();

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Dictionary<string, int> ObtenerItems()
    {
        return items;
    }

    public void AgregarItem(string nombre, int cantidad = 1)
    {
        if (items.ContainsKey(nombre))
        {
            items[nombre] += cantidad;
        }
        else
        {
            items[nombre] = cantidad;
        }

        Debug.Log($"Obtuviste {nombre} x{cantidad}");
    }

    public bool TieneItem(string nombre)
    {
        return items.ContainsKey(nombre) && items[nombre] > 0;
    }

    public void UsarItem(string nombre)
    {
        if (TieneItem(nombre))
        {
            items[nombre]--;

            Debug.Log($"Usaste {nombre}");

            if (items[nombre] <= 0)
            {
                items.Remove(nombre);
            }
        }
    }
}