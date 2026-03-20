using UnityEngine;

public class EncenderLuz : MonoBehaviour
{

    public GameObject luz;
    //public void ActivarLuz()
    //{
    //    luz.SetActive(true);
    //}
    private void Start()
    {
        Debug.Log("Inicio poste");
    }
    public void ActivarLuz()
    {
        luz.SetActive(true);

    }
}
