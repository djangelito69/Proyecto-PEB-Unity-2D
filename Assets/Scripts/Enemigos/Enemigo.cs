using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    [SerializeField] private DatosEnemigos.TipoEnemigo tipoEnemigo;
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float distanciaDeteccion = 4f;


    private Transform jugador;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool enCombate = false;
    private DatosCombate datosCombate;

    public Enemigo enemigoActual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Obtener datos del enemigo según su tipo
        datosCombate = DatosEnemigos.ObtenerDatos(tipoEnemigo);

        // Aplicar sprite del enemigo
        if (datosCombate.sprite != null)
        {
            spriteRenderer.sprite = datosCombate.sprite;
        }
        else
        {
            Debug.LogWarning($"El sprite para {datosCombate.nombre} no está asignado en Resources");
        }

        // Buscar al jugador por tag
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player'");
        }
    }

    void FixedUpdate()
    {
        // Si está en combate o no hay jugador, no moverse
        if (enCombate || jugador == null) return;

        float distancia = Vector2.Distance(rb.position, jugador.position);

        // Si el jugador está dentro del rango de detección, perseguirlo
        if (distancia <= distanciaDeteccion)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb.linearVelocity = direccion * velocidad;

            // Voltear sprite según la dirección
            if (direccion.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direccion.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            // Detener movimiento si el jugador está fuera de rango
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enCombate = true;
            rb.linearVelocity = Vector2.zero;

            GestorEnemigos.instancia.enemigoEnMapa = gameObject;

            GestorEnemigos.instancia.EstablecerEnemigo(tipoEnemigo, datosCombate);

            SceneManager.LoadScene("Combate");
        }
    }

    // Para poder ajustar el tipo de enemigo desde el inspector
    public void EstablecerTipoEnemigo(DatosEnemigos.TipoEnemigo tipo)
    {
        tipoEnemigo = tipo;
    }

    // Método para obtener los datos del enemigo (útil para debug)
    public DatosCombate ObtenerDatos()
    {
        return datosCombate;
    }
}