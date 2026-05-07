using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario instancia;

    public List<Consumible> consumibles = new List<Consumible>();

    public ObjetoInventario[] objetos =
        new ObjetoInventario[4];

    private void Awake()
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

    int ObtenerIndicePorTipo(TipoObjeto tipo)
    {
        switch (tipo)
        {
            case TipoObjeto.Vida:
                return 0;

            case TipoObjeto.Ataque:
                return 1;

            case TipoObjeto.Velocidad:
                return 2;

            case TipoObjeto.PA:
                return 3;
        }

        return -1;
    }

    public void AgregarObjeto(ObjetoInventario nuevoObjeto)
    {
        int index =
            ObtenerIndicePorTipo(nuevoObjeto.tipo);

        if (index == -1)
            return;

        // Si ya había objeto en ese slot
        if (objetos[index] != null)
        {
            RemoverBonificaciones(objetos[index]);
        }

        // Guardar nuevo objeto
        objetos[index] = nuevoObjeto;

        // Aplicar bonus
        AplicarBonificaciones(nuevoObjeto);

        UIInventario.instancia.ActualizarUIObjetos();

        Debug.Log($"Objeto equipado: {nuevoObjeto.nombre}");
    }

    void AplicarBonificaciones(ObjetoInventario objeto)
    {
        GestorExperiencia.instancia
            .AñadirVidaMaxima(objeto.bonusVida);

        GestorExperiencia.instancia
            .AñadirPAMaximo(objeto.bonusPA);

        GestorExperiencia.instancia
            .AñadirDaño(objeto.bonusAtaque);
    }

    void RemoverBonificaciones(ObjetoInventario objeto)
    {
        GestorExperiencia.instancia
            .AñadirVidaMaxima(-objeto.bonusVida);

        GestorExperiencia.instancia
            .AñadirPAMaximo(-objeto.bonusPA);

        GestorExperiencia.instancia
            .AñadirDaño(-objeto.bonusAtaque);
    }

    public void UsarConsumible(int index)
    {
        if (index < 0 || index >= consumibles.Count)
            return;

        Consumible c = consumibles[index];

        if (GestorExperiencia.instancia == null)
        {
            Debug.LogError("GestorExperiencia es null");
            return;
        }

        // Obtener stats actuales
        int vidaActual =
            GestorExperiencia.instancia.ObtenerVidaActual();

        int paActual =
            GestorExperiencia.instancia.ObtenerPAActual();

        DatosCombate datos =
            GestorExperiencia.instancia.ObtenerDatosActuales();

        // CURAR VIDA
        vidaActual += c.curacionVida;

        if (vidaActual > datos.vida)
        {
            vidaActual = datos.vida;
        }

        // RECUPERAR PA
        paActual += c.recuperacionPA;

        if (paActual > datos.pa)
        {
            paActual = datos.pa;
        }

        // GUARDAR NUEVOS VALORES
        GestorExperiencia.instancia
            .EstablecerVidaActual(vidaActual);

        GestorExperiencia.instancia
            .EstablecerPAActual(paActual);

        Debug.Log($"Usaste {c.nombre}");

        // REDUCIR CANTIDAD
        c.cantidad--;

        if (c.cantidad <= 0)
        {
            consumibles.RemoveAt(index);
        }

        // ACTUALIZAR UI
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