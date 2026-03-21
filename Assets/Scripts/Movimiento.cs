using TMPro;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 5f;

    public Rigidbody2D rb;
    private Vector2 mover;

    public float Vida;
    public float VidaMax;
    public BarraDeVida BarraDeVida;

    public EncenderLuz EncenderLuz;

    public AudioSource MuerteSfx;
    private bool muerto = false;

    int NumComida = 0;
    int NumBilletes = 0;
    int NumPalomas = 0;

    public TextMeshProUGUI TextoComida;
    public TextMeshProUGUI TextoPalomas;
    public TextMeshProUGUI TextoBilletes;


    float Daño = 1f;
    float Curar = 2f;


    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale;

        Vida = VidaMax;
        if (BarraDeVida != null)
        {
            BarraDeVida.VidaMax(VidaMax);
            BarraDeVida.ActualizarVida(Vida);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("Colisionando con enemigo");
            DañoVida(Daño);

        }

        //if (collision.gameObject.CompareTag("Curar"))
        //{
        //    CurarVida(Curar);
        //}

        if (collision.gameObject.CompareTag("Poste"))
        {
            EncenderLuz.ActivarLuz();
        }

        if (collision.gameObject.CompareTag("Comida"))
        {
            NumComida++;
            TextoComida.text = "Comida: " + NumComida.ToString();

            CurarVida(Curar);

            Destroy(collision.gameObject);

            Debug.Log("Comida recogida: " + NumComida);
        }

        if (collision.gameObject.CompareTag("Billetes"))
        {
            NumBilletes++;
            TextoBilletes.text = "Billetes: " + NumBilletes.ToString();

            Destroy(collision.gameObject);

            Debug.Log("Billetes recogidos: " + NumBilletes);
        }

        if (collision.gameObject.CompareTag("Palomas"))
        {
            NumPalomas++;
            TextoPalomas.text = "Palomas: " + NumPalomas.ToString();

            DañoVida(Daño);

            Destroy(collision.gameObject);

            Debug.Log("Palomas recogidas: " + NumPalomas);
        }

    }

    public void DañoVida(float daño)
    {
        if (muerto) return;

        Vida -= daño;
        Vida = Mathf.Clamp(Vida, 0, VidaMax);

        BarraDeVida.ActualizarVida(Vida);

        if (Vida <= 0)
        {
            Muerte();
        }
    }

    void Muerte()
    {
        muerto = true;

        if (MuerteSfx != null)
        {
            MuerteSfx.Play();
        }

        Debug.Log("Player ha muerto");
    }

    public void CurarVida(float curar)
    {
        if (muerto) return;

        Vida += curar;
        Vida = Mathf.Clamp(Vida, 0, VidaMax);

        BarraDeVida.ActualizarVida(Vida);
    }


}
