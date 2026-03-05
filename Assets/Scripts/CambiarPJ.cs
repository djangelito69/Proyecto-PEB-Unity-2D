using UnityEngine;

public class CambiarPJ : MonoBehaviour
{
    public GameObject[] personajes;
    private int pjAct = 0;

    void Start()
    {
        ActivarPJ(0);
    }

    public void ActivarPJ(int pj)
    {
        for (int i = 0; i < personajes.Length; i++)
        {
            personajes[i].SetActive(false);
        }

        personajes[pj].SetActive(true);
        pjAct = pj;
    }
}