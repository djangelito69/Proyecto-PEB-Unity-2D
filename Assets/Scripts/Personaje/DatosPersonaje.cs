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

    public static DatosCombate ObtenerDatos(TipoPersonaje tipo)
    {
        switch (tipo)
        {
            case TipoPersonaje.Gato:
                return new DatosCombate
                {
                    nombre = "Gato",
                    vida = 15,
                    pa = 10,
                    dañoBasico = 3,
                    dañoEspecial = 6,
                    costoBasico = 2,
                    costoEspecial = 5,
                    recuperacionPA = 2
                };

            case TipoPersonaje.Perro:
                return new DatosCombate
                {
                    nombre = "Perro",
                    vida = 20,
                    pa = 7,
                    dañoBasico = 4,
                    dañoEspecial = 7,
                    costoBasico = 2,
                    costoEspecial = 5,
                    recuperacionPA = 2
                };

            case TipoPersonaje.Raton:
                return new DatosCombate
                {
                    nombre = "Ratón",
                    vida = 10,
                    pa = 15,
                    dañoBasico = 2,
                    dañoEspecial = 8,
                    costoBasico = 1,
                    costoEspecial = 4,
                    recuperacionPA = 3
                };
        }

        return null;
    }

    public static void ElegirPersonaje(TipoPersonaje tipo)
    {
        PersonajeSeleccionado = tipo;
        Debug.Log($"Personaje seleccionado: {PersonajeSeleccionado}");
    }
}