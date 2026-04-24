using UnityEngine;

public class ControlMapa : MonoBehaviour
{
    void Start()
    {
        if (GestorEnemigos.instancia.EnemigoDerrotado)
        {
            if (GestorEnemigos.instancia.enemigoEnMapa != null)
            {
                Destroy(GestorEnemigos.instancia.enemigoEnMapa);
                GestorEnemigos.instancia.enemigoEnMapa = null;
            }

            GestorEnemigos.instancia.EnemigoDerrotado = false;
        }
    }
}