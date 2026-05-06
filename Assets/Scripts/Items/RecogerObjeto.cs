using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    [SerializeField] private string idObjeto;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Inventario.instancia.ColeccionarObjeto(idObjeto))
            {
                // Aplicar bonificaciones automáticamente
                var datosObjeto = Inventario.instancia.ObtenerDataObjeto(idObjeto);
                var jugador = FindObjectOfType<EstadisticasJugador>();

                if (jugador != null)
                {
                    if (datosObjeto.bonusAtaque > 0)
                        jugador.AumentarAtaque(datosObjeto.bonusAtaque);
                    if (datosObjeto.bonusVelocidad > 0)
                        jugador.AumentarVelocidad(datosObjeto.bonusVelocidad);
                    if (datosObjeto.bonusVida > 0)
                        jugador.AumentarVidaMaxima(datosObjeto.bonusVida);
                    if (datosObjeto.bonusPA > 0)
                        jugador.AumentarPAMaximo(datosObjeto.bonusPA);
                }

                Destroy(gameObject);
            }
        }
    }
}
