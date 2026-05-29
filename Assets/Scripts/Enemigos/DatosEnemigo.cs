using UnityEngine;

public static class DatosEnemigos
{
    public enum TipoEnemigo
    {
        BoteBasura,
        BolsaBasuraBlanca,
        BolsaBasuraNegra,
        CajaCarton,
        LaCosa
    }

    public static DatosCombate ObtenerDatos(TipoEnemigo tipo)
    {
        switch (tipo)
        {
            case TipoEnemigo.BolsaBasuraNegra:
                return new DatosCombate
                {
                    nombre = "Bolsa de Basura Negra",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/BolsaBasuraNegra"),

                    vida = 35,
                    pa = 14,

                    dañoBasico = 4,
                    dañoEspecial = 8,

                    costoBasico = 2,
                    costoEspecial = 5,

                    recuperacionPA = 3
                };

            case TipoEnemigo.BoteBasura:
                return new DatosCombate
                {
                    nombre = "Bote de Basura",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/BoteBasura"),

                    vida = 45,
                    pa = 8,

                    dañoBasico = 3,
                    dañoEspecial = 5,

                    costoBasico = 2,
                    costoEspecial = 4,

                    recuperacionPA = 2
                };

            case TipoEnemigo.BolsaBasuraBlanca:
                return new DatosCombate
                {
                    nombre = "Bolsa de Basura Blanca",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/BolsaBasuraBlanca"),

                    vida = 50,
                    pa = 10,

                    dañoBasico = 4,
                    dañoEspecial = 7,

                    costoBasico = 2,
                    costoEspecial = 5,

                    recuperacionPA = 2
                };

            case TipoEnemigo.CajaCarton:
                return new DatosCombate
                {
                    nombre = "Caja de Cartón",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/CajaCarton"),

                    vida = 65,
                    pa = 15,

                    dañoBasico = 5,
                    dañoEspecial = 10,

                    costoBasico = 2,
                    costoEspecial = 6,

                    recuperacionPA = 3
                };

            case TipoEnemigo.LaCosa:
                return new DatosCombate
                {
                    nombre = "La Cosa",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/BOSS"),

                    vida = 120,
                    pa = 20,

                    dañoBasico = 7,
                    dañoEspecial = 14,

                    costoBasico = 2,
                    costoEspecial = 7,

                    recuperacionPA = 4
                };
        }

        return null;
    }

    public static int ObtenerExperiencia(TipoEnemigo tipo)
    {
        return tipo switch
        {
            TipoEnemigo.BoteBasura => 25,
            TipoEnemigo.BolsaBasuraBlanca => 50,
            TipoEnemigo.BolsaBasuraNegra => 75,
            TipoEnemigo.CajaCarton => 100,
            TipoEnemigo.LaCosa => 1000,
            _ => 10
        };
    }
}