using UnityEngine;

[System.Serializable]
public class ConsumibleData
{
    public string id; // Identificador único (ej: "pizza", "manzana")
    public string nombre;
    public Sprite imagen;
    public string descripcion;
    public int curacionVida;
    public int recuperacionPA;
}
