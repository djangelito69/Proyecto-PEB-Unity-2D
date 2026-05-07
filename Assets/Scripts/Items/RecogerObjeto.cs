using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public ObjetoInventario objeto;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario.instancia
                .AgregarObjeto(objeto);

            Destroy(gameObject);
        }
    }
}