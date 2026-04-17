using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CambioObjetos : MonoBehaviour
{
    public float intervalo = 10f;
    private float tiempo;

    public TMP_Text textoTiempo; // arrastras el texto aquí desde el inspector

    private List<GameObject> comida;
    private List<GameObject> billetes;
    private List<GameObject> palomas;

    private int estado = 0;

    void Start()
    {
        comida = new List<GameObject>(GameObject.FindGameObjectsWithTag("Comida"));
        billetes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Billetes"));
        palomas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Palomas"));

        ActivarGrupo(estado);
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        float restante = intervalo - tiempo;

        // Mostrar en pantalla (redondeado)
        textoTiempo.text = "Cambio en: " + Mathf.Ceil(restante).ToString();

        if (tiempo >= intervalo)
        {
            tiempo = 0f;

            estado++;
            if (estado > 2) estado = 0;

            ActivarGrupo(estado);
        }
    }

    void ActivarGrupo(int estado)
    {
        SetActivo(comida, false);
        SetActivo(billetes, false);
        SetActivo(palomas, false);

        if (estado == 0) SetActivo(comida, true);
        if (estado == 1) SetActivo(billetes, true);
        if (estado == 2) SetActivo(palomas, true);
    }

    void SetActivo(List<GameObject> lista, bool valor)
    {
        foreach (GameObject obj in lista)
        {
            if (obj != null) // <- IMPORTANTE
            {
                obj.SetActive(valor);
            }
        }
    }
}