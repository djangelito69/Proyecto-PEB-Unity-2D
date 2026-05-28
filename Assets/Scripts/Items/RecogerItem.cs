using UnityEngine;

public class RecogerItem : MonoBehaviour
{
    public Consumible consumible;
    public int cantidad = 1;

    [Header("Audio")]
    public AudioClip sonidoRecoger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario.instancia.AgregarItem(
                consumible,
                cantidad
            );

            // Reproducir sonido
            if (sonidoRecoger != null)
            {
                AudioSource.PlayClipAtPoint(
                    sonidoRecoger,
                    transform.position
                );
            }

            Destroy(gameObject);
        }
    }
}