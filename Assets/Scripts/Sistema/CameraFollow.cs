using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform personaje;
    public bool seguir;

    void LateUpdate()
    {
        if (seguir && personaje != null)
        {
            Vector3 posicion = personaje.position;
            posicion.z = -10f;

            transform.position = posicion;
        }
    }
}