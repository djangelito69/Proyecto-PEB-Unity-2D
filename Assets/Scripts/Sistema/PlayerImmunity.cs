using UnityEngine;
using System.Collections;

/// <summary>
/// Gestiona el estado de inmunidad del jugador tras entrar en combate.
/// Mientras está inmune:
/// - No puede ser detectado por enemigos
/// - Su sprite cambia de color temporalmente
/// - Ignora colisiones con enemigos
/// </summary>
public class PlayerImmunity : MonoBehaviour
{
    [Header("Configuración de Inmunidad")]
    [SerializeField] private float duracionInmunidad = 2f;
    [SerializeField] private Color colorInmunidad = new Color(1f, 0.5f, 0.5f, 1f);
    [SerializeField] private float velocidadParpadeo = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;

    private bool esInmune = false;
    private Coroutine corrutinaPistolaje;

    // Collider del jugador
    private Collider2D playerCollider;

    // Layer original
    private int layerOriginal;

    public bool EsInmune => esInmune;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }

        playerCollider = GetComponent<Collider2D>();

        // Guardar layer original
        layerOriginal = gameObject.layer;
    }

    /// <summary>
    /// Activa inmunidad temporal.
    /// </summary>
    public void ActivarInmunidad()
    {
        if (esInmune)
            return;

        Debug.Log("PlayerImmunity: Inmunidad activada");

        esInmune = true;

        // Cambiar layer para ignorar enemigos
        gameObject.layer = LayerMask.NameToLayer("JugadorInmune");

        // Detener corrutina previa
        if (corrutinaPistolaje != null)
        {
            StopCoroutine(corrutinaPistolaje);
        }

        corrutinaPistolaje = StartCoroutine(ParpadearDuranteInmunidad());
    }

    /// <summary>
    /// Parpadeo visual durante inmunidad.
    /// </summary>
    private IEnumerator ParpadearDuranteInmunidad()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionInmunidad)
        {
            if (spriteRenderer == null)
            {
                esInmune = false;
                yield break;
            }

            spriteRenderer.color =
                ((tiempoTranscurrido % (velocidadParpadeo * 2)) < velocidadParpadeo)
                ? colorInmunidad
                : colorOriginal;

            tiempoTranscurrido += Time.deltaTime;

            yield return null;
        }

        DesactivarInmunidad();
    }

    /// <summary>
    /// Desactiva inmunidad manualmente.
    /// </summary>
    public void DesactivarInmunidad()
    {
        if (corrutinaPistolaje != null)
        {
            StopCoroutine(corrutinaPistolaje);
            corrutinaPistolaje = null;
        }

        esInmune = false;

        // Restaurar layer original
        gameObject.layer = layerOriginal;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorOriginal;
        }

        Debug.Log("PlayerImmunity: Inmunidad desactivada");
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}