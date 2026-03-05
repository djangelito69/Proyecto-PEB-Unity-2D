using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void VidaMax(float vidaMax) 
    {
        slider.maxValue = vidaMax;
    }

    public void ActualizarVida(float numVida) 
    {
        slider.value = numVida;
    }

    public void InicializarBarra(float numVida) 
    {
        VidaMax(numVida);
        ActualizarVida(numVida);
    }
}
