using UnityEngine;

public class RecogerItem : MonoBehaviour
{
    public string nombreItem;
    public int cantidad = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario.instancia.AgregarItem(
                nombreItem,
                cantidad
            );

            Destroy(gameObject);
        }
    }
}