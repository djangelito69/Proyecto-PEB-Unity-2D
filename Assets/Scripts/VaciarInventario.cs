using UnityEngine;

public class VaciarInventario : MonoBehaviour
{
    //public Movimiento mov;
    public CambiarPJ cambiarPJ;

    public void VaciarInv()
    {
        // Verifica si ya puede usar el personaje 4
        //if (mov.CantObj < cambiarPJ.objetosMinimos)
        {
            Debug.Log("Aun no desbloqueas esta opcion");
            return;
        }

        //mov.NumComida = 0;
        //mov.NumBilletes = 0;
        //mov.NumPalomas = 0;
        //
        //mov.TextoComida.text = "Comida: 0";
        //mov.TextoBilletes.text = "Billetes: 0";
        //mov.TextoPalomas.text = "Palomas: 0";

        //Debug.Log("Inventario vaciado");
    }
}