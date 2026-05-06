using UnityEngine;
using UnityEngine.UI;

public class PanelInventario : MonoBehaviour
{
    [SerializeField] private Button pestañaConsumibles;
    [SerializeField] private Button pestañaObjetos;
    [SerializeField] private Transform contenedorConsumibles;
    [SerializeField] private Transform contenedorObjetos;
    [SerializeField] private Button botonCerrar;

    [SerializeField] private ElementoConsumibleUI prefabElementoConsumible;
    [SerializeField] private ElementoObjetoUI prefabElementoObjeto;

    private CanvasGroup canvasGroup;
    private bool estaAbierto = false;

    void OnEnable()
    {
        pestañaConsumibles.onClick.AddListener(MostrarConsumibles);
        pestañaObjetos.onClick.AddListener(MostrarObjetos);
        botonCerrar.onClick.AddListener(Cerrar);

        Inventario.instancia.OnInventarioActualizado += ActualizarUI;
    }

    void OnDisable()
    {
        pestañaConsumibles.onClick.RemoveListener(MostrarConsumibles);
        pestañaObjetos.onClick.RemoveListener(MostrarObjetos);
        botonCerrar.onClick.RemoveListener(Cerrar);

        Inventario.instancia.OnInventarioActualizado -= ActualizarUI;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        gameObject.SetActive(false);
        ConstruirConsumibles();
        ConstruirObjetos();
    }

    void ConstruirConsumibles()
    {
        // Limpiar contenedor
        foreach (Transform child in contenedorConsumibles)
            Destroy(child.gameObject);

        var consumibles = Inventario.instancia.ObtenerConsumiblesDisponibles();
        foreach (var consumible in consumibles)
        {
            var elemento = Instantiate(prefabElementoConsumible, contenedorConsumibles);
            elemento.Inicializar(consumible);
        }
    }

    void ConstruirObjetos()
    {
        // Limpiar contenedor
        foreach (Transform child in contenedorObjetos)
            Destroy(child.gameObject);

        var objetos = Inventario.instancia.ObtenerObjetosDisponibles();
        foreach (var objeto in objetos)
        {
            var elemento = Instantiate(prefabElementoObjeto, contenedorObjetos);
            elemento.Inicializar(objeto);
        }
    }

    public void Abrir()
    {
        gameObject.SetActive(true);
        estaAbierto = true;
        MostrarConsumibles();
        ActualizarUI();
    }

    public void Cerrar()
    {
        estaAbierto = false;
        gameObject.SetActive(false);
    }

    public bool EstaAbierto => estaAbierto;

    void MostrarConsumibles()
    {
        contenedorConsumibles.gameObject.SetActive(true);
        contenedorObjetos.gameObject.SetActive(false);
        pestañaConsumibles.interactable = false;
        pestañaObjetos.interactable = true;
    }

    void MostrarObjetos()
    {
        contenedorConsumibles.gameObject.SetActive(false);
        contenedorObjetos.gameObject.SetActive(true);
        pestañaConsumibles.interactable = true;
        pestañaObjetos.interactable = false;
    }

    void ActualizarUI()
    {
        // Actualizar todos los elementos de consumibles visibles
        var elementosConsumibles = contenedorConsumibles.GetComponentsInChildren<ElementoConsumibleUI>();
        foreach (var elemento in elementosConsumibles)
            elemento.ActualizarCantidad();

        // Actualizar todos los elementos de objetos visibles
        var elementosObjetos = contenedorObjetos.GetComponentsInChildren<ElementoObjetoUI>();
        foreach (var elemento in elementosObjetos)
            elemento.ActualizarCantidad();
    }
}