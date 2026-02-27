using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{
    public float Speed = 2f;
    public float distanciaDeteccion = 4f;
    public Transform personaje;
    public string escenaCombate = "EscenaCombate";

    private Rigidbody2D rb;
    private bool enCombate = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (enCombate) return;

        float distancia = Vector2.Distance(rb.position, personaje.position);

        if (distancia <= distanciaDeteccion)
        {
            Vector2 direccion = (personaje.position - transform.position).normalized;
            rb.MovePosition(rb.position + direccion * Speed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enCombate = true;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
