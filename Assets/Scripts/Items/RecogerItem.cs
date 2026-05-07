using UnityEngine;

public class RecogerItem : MonoBehaviour
{
    public Consumible consumible;
    public int cantidad = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario.instancia.AgregarItem(
                consumible,
                cantidad
            );

            Destroy(gameObject);
        }
    }
}