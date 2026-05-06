using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementoConsumibleUI : MonoBehaviour
{
    [SerializeField] private Image imagenConsumible;
    [SerializeField] private TextMeshProUGUI textoCantidad;
    [SerializeField] private Button boton;

    private ConsumibleData datosConsumible;

    void OnEnable()
    {
        boton.onClick.AddListener(AlSeleccionar);
        Inventario.instancia.OnConsumibleCambiado += ActualizarSiEsEste;
    }

    void OnDisable()
    {
        boton.onClick.RemoveListener(AlSeleccionar);
        Inventario.instancia.OnConsumibleCambiado -= ActualizarSiEsEste;
    }

    public void Inicializar(ConsumibleData datos)
    {
        datosConsumible = datos;
        imagenConsumible.sprite = datos.imagen;
        ActualizarCantidad();
    }

    public void ActualizarCantidad()
    {
        int cantidad = Inventario.instancia.ObtenerCantidadConsumible(datosConsumible.id);
        textoCantidad.text = $"x{cantidad}";
    }

    void ActualizarSiEsEste(string idConsumible, int nuevaCantidad)
    {
        if (idConsumible == datosConsumible.id)
            ActualizarCantidad();
    }

    void AlSeleccionar()
    {
        int cantidad = Inventario.instancia.ObtenerCantidadConsumible(datosConsumible.id);
        if (cantidad <= 0)
        {
            Debug.Log($"No tienes {datosConsumible.nombre}");
            return;
        }

        // Abrir panel de decisión (usar o tirar)
        UIMenuConsumible.instancia.Mostrar(datosConsumible);
    }
}
