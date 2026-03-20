using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CambioLuz : MonoBehaviour
{
    public Light2D luzGlobal;
    public Color colorInicio = Color.white;
    public Color colorFinal = Color.purple;

    public float duracion = 5f;

    private float tiempo;

    void Update()
    {
        tiempo += Time.deltaTime;
        float t = tiempo / duracion;

        luzGlobal.color = Color.Lerp(colorInicio, colorFinal, t);
    }
}
