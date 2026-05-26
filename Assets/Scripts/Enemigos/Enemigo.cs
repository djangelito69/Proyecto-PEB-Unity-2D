using UnityEngine;
using System.Collections; // Necesario para las corrutinas

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
    private PlayerImmunity playerImmunity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        datosCombate = DatosEnemigos.ObtenerDatos(tipoEnemigo);

        if (datosCombate.sprite != null)
        {
            spriteRenderer.sprite = datosCombate.sprite;
        }
        else
        {
            Debug.LogWarning($"El sprite para {datosCombate.nombre} no está asignado en Resources");
        }

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
            playerImmunity = jugadorObj.GetComponent<PlayerImmunity>();
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player'");
        }
    }

    void FixedUpdate()
    {
        // FRENO MAESTRO: Si estás en la pantalla de combate, los enemigos del mapa se congelan
        if (GestorCombateGlobal.instancia != null && GestorCombateGlobal.instancia.combateEnTransicion)
        {
            rb.linearVelocity = Vector2.zero; // Detiene su movimiento en seco
            return;
        }

        // Si el jugador está inmune o ya en combate, no perseguir
        if (enCombate || jugador == null)
            return;

        // Si el jugador tiene inmunidad, no detectarlo
        if (playerImmunity != null && playerImmunity.EsInmune)
            return;

        float distancia = Vector2.Distance(rb.position, jugador.position);

        if (distancia <= distanciaDeteccion)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb.linearVelocity = direccion * velocidad;

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
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // 1. CANDADO GLOBAL: Si CUALQUIER enemigo ya inició la transición, ignoramos esto por completo
        if (GestorCombateGlobal.instancia != null && GestorCombateGlobal.instancia.combateEnTransicion)
        {
            return;
        }

        // 2. Si el jugador está inmune, ignorar la colisión
        if (playerImmunity != null && playerImmunity.EsInmune)
        {
            Debug.Log($"Enemigo {datosCombate.nombre}: Jugador inmune, ignorando colisión");
            return;
        }

        // 3. Si este enemigo ya está en combate, no hacer nada
        if (enCombate)
        {
            return;
        }

        // GestorCombateGlobal es un singleton persistente que existe desde el inicio
        if (GestorCombateGlobal.instancia == null)
        {
            Debug.LogError("Enemigo: GestorCombateGlobal no inicializado. Asegúrate de que está en la escena inicial.");
            return;
        }

        // 4. Intentar iniciar combate a través del gestor global
        bool combateIniciado = GestorCombateGlobal.instancia.IntentarIniciarCombate(tipoEnemigo, datosCombate, gameObject);

        if (combateIniciado)
        {
            enCombate = true;
            rb.linearVelocity = Vector2.zero;

            // 5. APAGAR FÍSICAS DE FORMA SEGURA (evita el crasheo de C++)
            StartCoroutine(DesactivarFisicasSeguro());
        }
    }

    /// <summary>
    /// Espera a que termine el cálculo de físicas del frame actual antes de apagar los componentes.
    /// Esto evita que el motor Box2D crashee al interrumpir una colisión en proceso.
    /// </summary>
    private IEnumerator DesactivarFisicasSeguro()
    {
        // Esperamos al final del ciclo de físicas
        yield return new WaitForFixedUpdate();

        if (col != null)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.simulated = false;
        }
    }

    public void EstablecerTipoEnemigo(DatosEnemigos.TipoEnemigo tipo)
    {
        tipoEnemigo = tipo;
    }

    public DatosCombate ObtenerDatos()
    {
        return datosCombate;
    }

    public void ReactivarEnemigo()
    {
        enCombate = false;

        if (col != null)
        {
            col.enabled = true;
        }

        if (rb != null)
        {
            rb.simulated = true;
        }
    }
}