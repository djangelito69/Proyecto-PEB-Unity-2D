using UnityEngine;

public class Colisiones : MonoBehaviour
{
    public CameraFollow camaraFollow;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Capsule"))
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            sr.color = Color.red;

            Debug.Log("Entro");

            if (camaraFollow != null)
                camaraFollow.seguir = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Capsule"))
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            sr.color = Color.red;

            Debug.Log("Esta en la colision");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Capsule"))
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            sr.color = Color.white;

            Debug.Log("Salio");

             if (camaraFollow != null)
                 camaraFollow.seguir = false;
        }
    }
}