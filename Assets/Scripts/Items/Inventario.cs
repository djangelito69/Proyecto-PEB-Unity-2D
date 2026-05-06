using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario instancia;

    [SerializeField] private ConsumibleData[] consumiblesDisponibles;
    [SerializeField] private ObjetoColeccionableData[] objetosDisponibles;

    // Diccionarios para almacenar cantidades
    private Dictionary<string, int> consumibles = new Dictionary<string, int>();
    private Dictionary<string, int> objetos = new Dictionary<string, int>();

    // Eventos para notificar cambios
    public event Action<string, int> OnConsumibleCambiado;
    public event Action<string, int> OnObjetoCambiado;
    public event Action<string> OnConsumibleUsado; // Para mostrar cambios de HP/PA
    public event Action OnInventarioActualizado;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            InicializarInventario();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InicializarInventario()
    {
        // Inicializar consumibles con cantidad 0
        foreach (var consumible in consumiblesDisponibles)
        {
            consumibles[consumible.id] = 0;
        }

        // Inicializar objetos con cantidad 0
        foreach (var objeto in objetosDisponibles)
        {
            objetos[objeto.id] = 0;
        }
    }

    // ==================== CONSUMIBLES ====================

    public ConsumibleData[] ObtenerConsumiblesDisponibles()
    {
        return consumiblesDisponibles;
    }

    public int ObtenerCantidadConsumible(string idConsumible)
    {
        return consumibles.ContainsKey(idConsumible) ? consumibles[idConsumible] : 0;
    }

    public ConsumibleData ObtenerDataConsumible(string idConsumible)
    {
        foreach (var consumible in consumiblesDisponibles)
        {
            if (consumible.id == idConsumible)
                return consumible;
        }
        return null;
    }

    public void AgregarConsumible(string idConsumible, int cantidad = 1)
    {
        if (!consumibles.ContainsKey(idConsumible))
            return;

        consumibles[idConsumible] += cantidad;
        OnConsumibleCambiado?.Invoke(idConsumible, consumibles[idConsumible]);
        OnInventarioActualizado?.Invoke();
        Debug.Log($"Obtuviste {idConsumible} x{cantidad}. Total: {consumibles[idConsumible]}");
    }

    public bool TieneConsumible(string idConsumible, int cantidad = 1)
    {
        return consumibles.ContainsKey(idConsumible) && consumibles[idConsumible] >= cantidad;
    }

    public void UsarConsumible(string idConsumible)
    {
        if (!TieneConsumible(idConsumible))
            return;

        consumibles[idConsumible]--;
        OnConsumibleCambiado?.Invoke(idConsumible, consumibles[idConsumible]);
        OnConsumibleUsado?.Invoke(idConsumible);
        OnInventarioActualizado?.Invoke();
        Debug.Log($"Usaste {idConsumible}. Quedan: {consumibles[idConsumible]}");
    }

    public void TirarConsumible(string idConsumible)
    {
        if (!TieneConsumible(idConsumible))
            return;

        consumibles[idConsumible]--;
        OnConsumibleCambiado?.Invoke(idConsumible, consumibles[idConsumible]);
        OnInventarioActualizado?.Invoke();
        Debug.Log($"Tiraste {idConsumible}. Quedan: {consumibles[idConsumible]}");
    }

    // ==================== OBJETOS COLECCIONABLES ====================

    public ObjetoColeccionableData[] ObtenerObjetosDisponibles()
    {
        return objetosDisponibles;
    }

    public int ObtenerCantidadObjeto(string idObjeto)
    {
        return objetos.ContainsKey(idObjeto) ? objetos[idObjeto] : 0;
    }

    public ObjetoColeccionableData ObtenerDataObjeto(string idObjeto)
    {
        foreach (var objeto in objetosDisponibles)
        {
            if (objeto.id == idObjeto)
                return objeto;
        }
        return null;
    }

    public bool ColeccionarObjeto(string idObjeto)
    {
        if (!objetos.ContainsKey(idObjeto))
            return false;

        // Los objetos solo se pueden coleccionar una vez
        if (objetos[idObjeto] > 0)
            return false;

        objetos[idObjeto] = 1;
        OnObjetoCambiado?.Invoke(idObjeto, 1);
        OnInventarioActualizado?.Invoke();
        Debug.Log($"Coleccionaste {idObjeto}");
        return true;
    }

    public bool TieneObjeto(string idObjeto)
    {
        return objetos.ContainsKey(idObjeto) && objetos[idObjeto] > 0;
    }

    // ==================== MÉTODOS ANTIGUOS (COMPATIBILIDAD) ====================

    public Dictionary<string, int> ObtenerItems()
    {
        var itemsCombinados = new Dictionary<string, int>();
        foreach (var item in consumibles)
            itemsCombinados[item.Key] = item.Value;
        foreach (var item in objetos)
            itemsCombinados[item.Key] = item.Value;
        return itemsCombinados;
    }

    public void AgregarItem(string nombre, int cantidad = 1)
    {
        AgregarConsumible(nombre, cantidad);
    }

    public bool TieneItem(string nombre)
    {
        return TieneConsumible(nombre) || TieneObjeto(nombre);
    }

    public void UsarItem(string nombre)
    {
        UsarConsumible(nombre);
    }
}
