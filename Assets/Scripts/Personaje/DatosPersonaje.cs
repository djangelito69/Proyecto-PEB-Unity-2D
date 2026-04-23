using UnityEngine;

public static class DatosPersonaje
{
    public enum TipoPersonaje
    {
        Gato,
        Perro,
        Raton
    }

    public static TipoPersonaje PersonajeSeleccionado { get; set; } = TipoPersonaje.Gato;

    public static void ElegirPersonaje(TipoPersonaje tipo)
    {
        PersonajeSeleccionado = tipo;
        Debug.Log($"Personaje seleccionado: {PersonajeSeleccionado}");
    }
}
