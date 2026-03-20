using UnityEngine;

public class PosteLuz : MonoBehaviour
{
    private Animator miAnimador;

    void Start()
    {
        miAnimador = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            miAnimador.SetTrigger("Encender");

            Debug.Log("¡Animación de luz activada!");
        }
    }
}
