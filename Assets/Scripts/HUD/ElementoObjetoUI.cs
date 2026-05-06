using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementoObjetoUI : MonoBehaviour
{
    [SerializeField] private Image imagenObjeto;
    [SerializeField] private TextMeshProUGUI textoCantidad;
    [SerializeField] private Button boton;

    private ObjetoColeccionableData datosObjeto;

    void OnEnable()
    {
        boton.onClick.AddListener(AlSeleccionar);
        Inventario.instancia.OnObjetoCambiado += ActualizarSiEsEste;
    }

    void OnDisable()
    {
        boton.onClick.RemoveListener(AlSeleccionar);
        Inventario.instancia.OnObjetoCambiado -= ActualizarSiEsEste;
    }

    public void Inicializar(ObjetoColeccionableData datos)
    {
        datosObjeto = datos;
        imagenObjeto.sprite = datos.imagen;
        ActualizarCantidad();
    }

    public void ActualizarCantidad()
    {
        int cantidad = Inventario.instancia.ObtenerCantidadObjeto(datosObjeto.id);
        textoCantidad.text = $"x{cantidad}";
    }

    void ActualizarSiEsEste(string idObjeto, int nuevaCantidad)
    {
        if (idObjeto == datosObjeto.id)
            ActualizarCantidad();
    }

    void AlSeleccionar()
    {
        // Los objetos coleccionables no se pueden usar, solo se muestran
        UIMenuConsumible.instancia.MostrarDetallesObjeto(datosObjeto);
    }
}
