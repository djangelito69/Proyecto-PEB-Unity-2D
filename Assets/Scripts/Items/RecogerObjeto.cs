using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public ObjetoInventario objeto;

    [Header("Audio")]
    public AudioClip sonidoRecoger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario.instancia
                .AgregarObjeto(objeto);

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