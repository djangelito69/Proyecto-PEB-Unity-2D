using UnityEngine;

public enum TipoObjeto
{
    Ataque,
    Vida,
    PA,
    Velocidad
}

[System.Serializable]
public class ObjetoInventario
{
    public string nombre;
    public string descripcion;

    public Sprite icono;

    public TipoObjeto tipo;

    public int bonusVida;
    public int bonusPA;
    public int bonusAtaque;
    public int bonusVelocidad;
}