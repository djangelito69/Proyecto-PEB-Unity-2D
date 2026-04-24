using UnityEngine;

public class Dialogo : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasdialogo;
    //public Movimiento movimiento;

    public void Aceptar()
    {
        canvasdialogo.SetActive(false);
        //movimiento.estado = 0;
        canvas.SetActive(true);
    }
}
