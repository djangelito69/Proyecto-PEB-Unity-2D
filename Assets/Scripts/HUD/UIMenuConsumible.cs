using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuConsumible : MonoBehaviour
{
    public static UIMenuConsumible instancia;

    [SerializeField] private GameObject panelMenu;
    [SerializeField] private Image imagenConsumible;
    [SerializeField] private TextMeshProUGUI nombreConsumible;
    [SerializeField] private TextMeshProUGUI descripcionConsumible;
    [SerializeField] private Button botonUsar;
    [SerializeField] private Button botonTirar;
    [SerializeField] private Button botonCerrar;

    // Panel de información para objetos coleccionables
    [SerializeField] private GameObject panelInfoObjeto;
    [SerializeField] private Image imagenObjeto;
    [SerializeField] private TextMeshProUGUI nombreObjeto;
    [SerializeField] private TextMeshProUGUI descripcionObjeto;
    [SerializeField] private TextMeshProUGUI infoBonus;
    [SerializeField] private Button botonCerrarObjeto;

    private ConsumibleData consumibleActual;
    private ObjetoColeccionableData objetoActual;

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        botonUsar.onClick.AddListener(Usar);
        botonTirar.onClick.AddListener(Tirar);
        botonCerrar.onClick.AddListener(CerrarMenu);
        botonCerrarObjeto.onClick.AddListener(CerrarMenu);
    }

    void OnDisable()
    {
        botonUsar.onClick.RemoveListener(Usar);
        botonTirar.onClick.RemoveListener(Tirar);
        botonCerrar.onClick.RemoveListener(CerrarMenu);
        botonCerrarObjeto.onClick.RemoveListener(CerrarMenu);
    }

    public void Mostrar(ConsumibleData datos)
    {
        consumibleActual = datos;
        panelMenu.SetActive(true);
        panelInfoObjeto.SetActive(false);

        imagenConsumible.sprite = datos.imagen;
        nombreConsumible.text = datos.nombre;
        descripcionConsumible.text = datos.descripcion;

        // Mostrar efectos
        string efectos = "";
        if (datos.curacionVida > 0)
            efectos += $"+{datos.curacionVida} HP\n";
        if (datos.recuperacionPA > 0)
            efectos += $"+{datos.recuperacionPA} PA";

        if (string.IsNullOrEmpty(efectos))
            efectos = "Sin efectos";

        descripcionConsumible.text += "\n\n" + efectos;
    }

    public void MostrarDetallesObjeto(ObjetoColeccionableData datos)
    {
        objetoActual = datos;
        panelMenu.SetActive(true);
        panelInfoObjeto.SetActive(true);

        imagenObjeto.sprite = datos.imagen;
        nombreObjeto.text = datos.nombre;
        descripcionObjeto.text = datos.descripcion;

        // Mostrar bonificaciones
        string bonos = "";
        if (datos.bonusAtaque > 0)
            bonos += $"+{datos.bonusAtaque} Ataque\n";
        if (datos.bonusVelocidad > 0)
            bonos += $"+{datos.bonusVelocidad} Velocidad\n";
        if (datos.bonusPA > 0)
            bonos += $"+{datos.bonusPA} PA\n";
        if (datos.bonusVida > 0)
            bonos += $"+{datos.bonusVida} Vida";

        if (string.IsNullOrEmpty(bonos))
            bonos = "Sin bonificaciones";

        infoBonus.text = bonos;
    }

    void Usar()
    {
        if (consumibleActual == null)
            return;

        // Usar el consumible
        Inventario.instancia.UsarConsumible(consumibleActual.id);

        // Aplicar efectos al jugador
        var jugador = FindObjectOfType<EstadisticasJugador>();
        if (jugador != null)
        {
            if (consumibleActual.curacionVida > 0)
                jugador.CurarVida(consumibleActual.curacionVida);
            if (consumibleActual.recuperacionPA > 0)
                jugador.RecuperarPA(consumibleActual.recuperacionPA);
        }

        Debug.Log($"Usaste {consumibleActual.nombre}");
        CerrarMenu();
    }

    void Tirar()
    {
        if (consumibleActual == null)
            return;

        Inventario.instancia.TirarConsumible(consumibleActual.id);
        Debug.Log($"Tiraste {consumibleActual.nombre}");
        CerrarMenu();
    }

    void CerrarMenu()
    {
        panelMenu.SetActive(false);
        consumibleActual = null;
        objetoActual = null;
    }
}
