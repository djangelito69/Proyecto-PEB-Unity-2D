using UnityEngine;

[System.Serializable]
public class Combate
{
    public string Nombre;

    public int vidaMaxima;
    public int vidaActual;

    public int PA_Maxima;
    public int PA_Actual;

    public int dañoBasico;
    public int dañoEspecial;

    public int PA_costoBasico;
    public int PA_costoEspecial;

    // Recuperación de stamina por turno (configurable por personaje/enemigo)
    public int PA_recuperacionPorTurno = 2;

    public bool EstaVivo => vidaActual > 0;

    public void RecibirDaño(int cantidad)
    {
        vidaActual = Mathf.Max(0, vidaActual - cantidad);
    }

    public bool TienePAParaBasico() => PA_Actual >= PA_costoBasico;
    public bool TienePAParaEspecial() => PA_Actual >= PA_costoEspecial;

    public void GastarPA(int cantidad)
    {
        PA_Actual = Mathf.Max(0, PA_Actual - cantidad);
    }

    public void RecuperarPA()
    {
        PA_Actual = Mathf.Min(PA_Maxima, PA_Actual + PA_recuperacionPorTurno);
    }

    public string ObtenerStats()
    {
        return $"{Nombre} | Vida: {vidaActual}/{vidaMaxima} | PA: {PA_Actual}/{PA_Maxima}";
    }
}