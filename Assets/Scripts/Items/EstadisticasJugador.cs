using UnityEngine;

public class EstadisticasJugador : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 100;
    [SerializeField] private int paMaximo = 50;

    private int vidaActual;
    private int paActual;

    // Estadísticas base
    private int ataque = 10;
    private int velocidad = 5;

    void Start()
    {
        vidaActual = vidaMaxima;
        paActual = paMaximo;
    }

    // ==================== VIDA ====================

    public void CurarVida(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMaxima);
        Debug.Log($"Vida actual: {vidaActual}/{vidaMaxima}");
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual = Mathf.Max(vidaActual - cantidad, 0);
        Debug.Log($"¡Recibiste {cantidad} de daño! Vida: {vidaActual}/{vidaMaxima}");

        if (vidaActual <= 0)
            Morir();
    }

    public int ObtenerVida()
    {
        return vidaActual;
    }

    public int ObtenerVidaMaxima()
    {
        return vidaMaxima;
    }

    // ==================== PA (Puntos de Acción) ====================

    public void RecuperarPA(int cantidad)
    {
        paActual = Mathf.Min(paActual + cantidad, paMaximo);
        Debug.Log($"PA actual: {paActual}/{paMaximo}");
    }

    public void GastarPA(int cantidad)
    {
        if (paActual >= cantidad)
        {
            paActual -= cantidad;
            Debug.Log($"Gastaste {cantidad} PA. Quedan: {paActual}");
            return;
        }

        Debug.Log("No tienes suficiente PA");
    }

    public int ObtenerPA()
    {
        return paActual;
    }

    public int ObtenerPAMaximo()
    {
        return paMaximo;
    }

    // ==================== ESTADÍSTICAS ====================

    public void AumentarAtaque(int cantidad)
    {
        ataque += cantidad;
        Debug.Log($"Ataque aumentado a: {ataque}");
    }

    public void AumentarVelocidad(int cantidad)
    {
        velocidad += cantidad;
        Debug.Log($"Velocidad aumentada a: {velocidad}");
    }

    public void AumentarVidaMaxima(int cantidad)
    {
        vidaMaxima += cantidad;
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMaxima);
        Debug.Log($"Vida máxima aumentada a: {vidaMaxima}");
    }

    public void AumentarPAMaximo(int cantidad)
    {
        paMaximo += cantidad;
        paActual = Mathf.Min(paActual + cantidad, paMaximo);
        Debug.Log($"PA máximo aumentado a: {paMaximo}");
    }

    public int ObtenerAtaque()
    {
        return ataque;
    }

    public int ObtenerVelocidad()
    {
        return velocidad;
    }

    // ==================== MUERTE ====================

    void Morir()
    {
        Debug.Log("¡El jugador ha muerto!");
        // Aquí puedes agregar lógica de game over
    }
}
