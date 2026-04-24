using UnityEngine;
public static class DatosEnemigos
{
    public enum TipoEnemigo
    {
        BoteBasura,
        BolsaBasuraBlanca,
        BolsaBasuraNegra,
        CajaCarton
    }

    public static DatosCombate ObtenerDatos(TipoEnemigo tipo)
    {
        switch (tipo)
        {
            case TipoEnemigo.BoteBasura:
                return new DatosCombate
                {
                    nombre = "Bote de Basura",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/BoteBasura"),
                    vida = 10,
                    pa = 6,
                    dañoBasico = 2,
                    dañoEspecial = 0,
                    costoBasico = 2,
                    costoEspecial = 2,
                    recuperacionPA = 1

                };

            case TipoEnemigo.BolsaBasuraBlanca:
                return new DatosCombate
                {
                    nombre = "Bolsa de Basura Blanca",
                    sprite = null,
                    vida = 18,
                    pa = 8,
                    dañoBasico = 4,
                    dañoEspecial = 6,
                    costoBasico = 2,
                    costoEspecial = 5,
                    recuperacionPA = 1
                };

            case TipoEnemigo.BolsaBasuraNegra:
                return new DatosCombate
                {
                    nombre = "Bolsa de Basura Negra",
                    sprite = Resources.Load<Sprite>("Sprites/Enemigos/BolsaBasuraNegra"),
                    vida = 5,
                    pa = 10,
                    dañoBasico = 5,
                    dañoEspecial = 6,
                    costoBasico = 3,
                    costoEspecial = 6,
                    recuperacionPA = 1
                };

            case TipoEnemigo.CajaCarton:
                return new DatosCombate
                {
                    nombre = "Caja de Cartón",
                    sprite = null,
                    vida = 25,
                    pa = 10,
                    dañoBasico = 5,
                    dañoEspecial = 9,
                    costoBasico = 3,
                    costoEspecial = 6,
                    recuperacionPA = 1
                };
        }

        return null;

    }
}