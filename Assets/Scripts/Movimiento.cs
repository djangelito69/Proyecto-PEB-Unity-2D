using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 5f;

    public Rigidbody2D rb;
    private Vector2 mover;

    public float Vida;
    public float VidaMax;
    public BarraDeVida BarraDeVida;

    float Daño = 1f;
    float Curar = 1f;


    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale;

        Vida = VidaMax;
        if (BarraDeVida != null)
        {
            BarraDeVida.InicializarBarra(Vida);
        }
    }

    void Update()
    {
        mover.x = Input.GetAxisRaw("Horizontal");
        mover.y = Input.GetAxisRaw("Vertical");

        if (mover.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (mover.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = mover.normalized * velocidad;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Enemigo")
        {
            DañoVida(Daño);
        }
        if (collision.collider.name == "Curar")
        {
            CurarVida(Curar);
        }

    }

    public void DañoVida(float daño)
    {
        Vida -= daño;
        BarraDeVida.ActualizarVida(Vida);
    }

    public void CurarVida(float curar)
    {
        Vida += curar;
        BarraDeVida.ActualizarVida(Vida);
    }
}
