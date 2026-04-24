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
        DontDestroyOnLoad(gameObject);
    }

    public void LimpiarEnemigo()
    {
        datosCombate = null;
    }
    public void EstablecerEnemigo(DatosEnemigos.TipoEnemigo tipo, DatosCombate datos)
    {
        tipoEnemigo = tipo;
        datosCombate = datos;
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
}