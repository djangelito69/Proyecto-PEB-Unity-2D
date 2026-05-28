using UnityEngine;

public class PartidaManager : MonoBehaviour
{
    public static PartidaManager instancia;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NuevaPartida()
    {
        Debug.Log("Iniciando nueva partida");

        if (Inventario.instancia != null)
        {
            Inventario.instancia.ReiniciarDatos();
        }

        if (GestorEnemigos.instancia != null)
        {
            GestorEnemigos.instancia.ReiniciarDatos();
        }

        if (GestorExperiencia.instancia != null)
        {
            GestorExperiencia.instancia.ReiniciarDatos();
        }

        if (MusicManager.instancia != null)
        {
            MusicManager.instancia.ReiniciarDatos();
        }
    }
}