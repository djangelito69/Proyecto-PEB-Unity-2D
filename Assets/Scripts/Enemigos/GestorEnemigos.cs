using UnityEngine;

public class GestorEnemigos : MonoBehaviour
{
    public static GestorEnemigos instancia { get; private set; }
    private DatosEnemigos.TipoEnemigo tipoEnemigo;
    private DatosCombate datosCombate;
    public bool EnemigoDerrotado = false;
    public GameObject enemigoEnMapa;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;

        // Desvincular del padre para convertirlo en un objeto raíz
        transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }

    public void LimpiarEnemigo()
    {
        datosCombate = null;
        enemigoEnMapa = null;
    }

    public void DestruirEnemigoDelMapa()
    {
        if (enemigoEnMapa != null)
        {
            Destroy(enemigoEnMapa);
            enemigoEnMapa = null;
            EnemigoDerrotado = true;
            Debug.Log("Enemigo destruido del mapa");
        }
    }

    public void EstablecerEnemigo(DatosEnemigos.TipoEnemigo tipo, DatosCombate datos)
    {
        tipoEnemigo = tipo;
        datosCombate = datos;
        EnemigoDerrotado = false;
        Debug.Log($"Enemigo establecido: {datos.nombre}");
    }

    public DatosEnemigos.TipoEnemigo ObtenerTipoEnemigo()
    {
        return tipoEnemigo;
    }

    public DatosCombate ObtenerDatosEnemigo()
    {
        return datosCombate;
    }

    public bool HayEnemigo()
    {
        return datosCombate != null;
    }

    public void ReiniciarDatos()
    {
        tipoEnemigo = default;

        datosCombate = null;

        enemigoEnMapa = null;

        EnemigoDerrotado = false;

        Debug.Log("GestorEnemigos reiniciado");
    }
}