using UnityEngine;

public class RecogerConsumible : MonoBehaviour
{
    [SerializeField] private string idConsumible;
    [SerializeField] private int cantidad = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario.instancia.AgregarConsumible(idConsumible, cantidad);
            Destroy(gameObject);
        }
    }
}
