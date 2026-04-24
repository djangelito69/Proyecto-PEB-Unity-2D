using UnityEngine;

public class CambiarPJ : MonoBehaviour
{
    public GameObject[] personajes;
    private int pjAct = 0;

    // Movimiento mov;
    public int objetosMinimos = 4;

    void Start()
    {
        ActivarPJ(0);
    }

    public void ActivarPJ(int pj)
    {
        
        if (pj == 3)
        {
            //if (mov.CantObj < objetosMinimos)
            {
                Debug.Log("No tienes suficientes objetos para desbloquear este personaje");
                return;
            }
        }

        for (int i = 0; i < personajes.Length; i++)
        {
            personajes[i].SetActive(false);
        }

        personajes[pj].SetActive(true);
        pjAct = pj;
    }
}