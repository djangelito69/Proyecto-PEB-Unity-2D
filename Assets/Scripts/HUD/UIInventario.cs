using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventario : MonoBehaviour
{
    public static UIInventario instancia;

    private Button botonSeleccionado;

    [SerializeField] private Color colorNormal = Color.white;
    [SerializeField] private Color colorSeleccionado = Color.yellow;

    [Header("Paneles")]
    public GameObject panelInventario;
    public GameObject panelConsumibles;
    public GameObject panelObjetos;

    [Header("OBJETOS")]

    public Button[] botonesObjetos;
    public Image[] iconosObjetos;
    public TMP_Text[] nombresObjetos;

    [Header("Slots")]
    public Button[] botonesSlots;

    [Header("UI Slot")]
    public Image[] iconos;
    public TMP_Text[] nombres;
    public TMP_Text[] cantidades;

    [Header("Sonidos")]
    public AudioSource audioSource;

    public AudioClip sonidoAbrir;
    public AudioClip sonidoClick;
    private int indiceSeleccionado = -1;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        panelInventario.SetActive(false);

        ActualizarUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool abrir = !panelInventario.activeSelf;

            panelInventario.SetActive(abrir);

            ReproducirSonido(sonidoAbrir);

            if (abrir)
            {
                MostrarConsumibles();
            }
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void MostrarConsumibles()
    {
        ReproducirSonido(sonidoClick);

        panelConsumibles.SetActive(true);
        panelObjetos.SetActive(false);
    }

    public void MostrarObjetos()
    {
        ReproducirSonido(sonidoClick);

        Debug.Log("ABRIENDO OBJETOS");

        panelConsumibles.SetActive(false);
        panelObjetos.SetActive(true);
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
                    "x" + lista[i].cantidad.ToString();

                iconos[i].sprite = lista[i].icono;

                int index = i;

                botonesSlots[i].onClick.RemoveAllListeners();

                botonesSlots[i].onClick.AddListener(() =>
                {
                    SeleccionarConsumible(index);
                });
            }
            else
            {
                botonesSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void ActualizarUIObjetos()
    {
        var lista = Inventario.instancia.objetos;

        for (int i = 0; i < botonesObjetos.Length; i++)
        {
            if (lista[i] != null)
            {
                botonesObjetos[i].gameObject.SetActive(true);

                nombresObjetos[i].text =
                    lista[i].nombre;

                iconosObjetos[i].sprite =
                    lista[i].icono;
            }
            else
            {
                nombresObjetos[i].text = "";

                iconosObjetos[i].sprite = null;
            }
        }
    }

    public void SeleccionarConsumible(int index)
    {
        indiceSeleccionado = index;

        // Restaurar anterior
        if (botonSeleccionado != null)
        {
            botonSeleccionado.image.color = colorNormal;
        }

        // Nuevo seleccionado
        botonSeleccionado = botonesSlots[index];

        botonSeleccionado.image.color = colorSeleccionado;
    }

    public void BotonUsar()
    {
        if (indiceSeleccionado != -1)
        {
            Inventario.instancia.UsarConsumible(
                indiceSeleccionado
            );
        }
    }

    public void BotonTirar()
    {
        if (indiceSeleccionado != -1)
        {
            Inventario.instancia.TirarConsumible(
                indiceSeleccionado
            );
        }
    }
}