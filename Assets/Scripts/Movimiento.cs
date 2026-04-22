using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasdialogo;

    public float velocidad = 5f;

    public Rigidbody2D rb;
    private Vector2 mover;

    public float Vida;
    public float VidaMax;
    public BarraDeVida BarraDeVida;

    public EncenderLuz EncenderLuz;

    public Temporizador Tempo;

    public AudioSource MuerteSfx;
    private bool muerto = false;

    public int NumComida = 0;
    public int NumBilletes = 0;
    public int NumPalomas = 0;
    public int NumPuntaje = 0;
    public int CantObj = 0;

    public TextMeshProUGUI TextoComida;
    public TextMeshProUGUI TextoPalomas;
    public TextMeshProUGUI TextoBilletes;
    public TextMeshProUGUI TextoPuntaje;


    float Daño = 1f;
    //float Curar = 2f;

    public int estado = 0;


    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale;

        Tempo = FindObjectOfType<Temporizador>();

        Vida = VidaMax;
        if (BarraDeVida != null)
        {
            BarraDeVida.VidaMax(VidaMax);
            BarraDeVida.ActualizarVida(Vida);
        }
    }

    void Update()
    {
        if (estado != 0) return;

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
        if (estado != 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = mover.normalized * velocidad;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Perro"))
        {
            estado = 1;
            Debug.Log("Colisionando con perro");
            canvas.SetActive(false);
            canvasdialogo.SetActive(true);

        }


        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Debug.Log("Colisionando con enemigo");
            DañoVida(Daño);

        }





        if (collision.gameObject.CompareTag("Poste"))
        {
            EncenderLuz.ActivarLuz();
        }



        if (collision.gameObject.CompareTag("Comida"))
        {
            NumComida++;
            TextoComida.text = "Comida: " + NumComida.ToString();

            NumPuntaje += 10;
            TextoPuntaje.text = "Puntaje: " + NumPuntaje.ToString();
            Debug.Log("Puntaje sumado: " + NumPuntaje);

            CantObj +=1;
            Debug.Log("Cantidad de Objetos: " + CantObj);

            //CurarVida(Curar);


            Tempo.SumarPuntos(10);


            Destroy(collision.gameObject);

            Debug.Log("Comida recogida: " + NumComida);
        }

        if (collision.gameObject.CompareTag("Billetes"))
        {
            NumBilletes++;
            TextoBilletes.text = "Billetes: " + NumBilletes.ToString();
            NumPuntaje += 20;
            TextoPuntaje.text = "Puntaje: " + NumPuntaje.ToString();
            Debug.Log("Puntaje sumado: " + NumPuntaje);

            CantObj += 1;
            Debug.Log("Cantidad de Objetos: " + CantObj);

            Tempo.SumarPuntos(20);


            Destroy(collision.gameObject);

            Debug.Log("Billetes recogidos: " + NumBilletes);
        }

        if (collision.gameObject.CompareTag("Palomas"))
        {
            NumPalomas++;
            TextoPalomas.text = "Palomas: " + NumPalomas.ToString();

            NumPuntaje += 30;
            TextoPuntaje.text = "Puntaje: " + NumPuntaje.ToString();
            Debug.Log("Puntaje sumado: " + NumPuntaje);

            CantObj += 1;
            Debug.Log("Cantidad de Objetos: " + CantObj);

            //DañoVida(Daño);


            Tempo.SumarPuntos(30);


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
