using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario instancia;

    public List<Consumible> consumibles = new List<Consumible>();

    private void Awake()
    {
        if (instancia == null)
            instancia = this;
    }

    public void AgregarItem(Consumible nuevoConsumible, int cantidad)
    {
        Consumible existente = consumibles.Find(
            c => c.nombre == nuevoConsumible.nombre
        );

        if (existente != null)
        {
            existente.cantidad += cantidad;
        }
        else
        {
            Consumible copia = new Consumible();

            copia.nombre = nuevoConsumible.nombre;
            copia.descripcion = nuevoConsumible.descripcion;
            copia.icono = nuevoConsumible.icono;
            copia.curacionVida = nuevoConsumible.curacionVida;
            copia.recuperacionPA = nuevoConsumible.recuperacionPA;
            copia.cantidad = cantidad;

            consumibles.Add(copia);
        }

        UIInventario.instancia.ActualizarUI();
    }

    public void UsarConsumible(int index)
    {
        if (index < 0 || index >= consumibles.Count)
            return;

        Consumible c = consumibles[index];

        GestorCombate.instancia.jugador.vidaActual += c.curacionVida;
        GestorCombate.instancia.jugador.paActual += c.recuperacionPA;

        c.cantidad--;

        if (c.cantidad <= 0)
        {
            consumibles.RemoveAt(index);
        }

        UIInventario.instancia.ActualizarUI();
    }

    public void TirarConsumible(int index)
    {
        if (index < 0 || index >= consumibles.Count)
            return;

        consumibles[index].cantidad--;

        if (consumibles[index].cantidad <= 0)
        {
            consumibles.RemoveAt(index);
        }

        UIInventario.instancia.ActualizarUI();
    }
}