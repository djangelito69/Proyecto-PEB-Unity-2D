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
                    vida = 40,
                    pa = 15,
                    dañoBasico = 4,
                    dañoEspecial = 8,
                    costoBasico = 2,
                    costoEspecial = 6,
                    recuperacionPA = 3
                };

            case TipoPersonaje.Perro:
                return new DatosCombate
                {
                    nombre = "Perro",
                    vida = 55,
                    pa = 12,
                    dañoBasico = 5,
                    dañoEspecial = 9,
                    costoBasico = 2,
                    costoEspecial = 6,
                    recuperacionPA = 2
                };

            case TipoPersonaje.Raton:
                return new DatosCombate
                {
                    nombre = "Ratón",
                    vida = 32,
                    pa = 20,
                    dañoBasico = 3,
                    dañoEspecial = 10,
                    costoBasico = 1,
                    costoEspecial = 5,
                    recuperacionPA = 4
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