using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform personaje;
    public bool seguir;

    void LateUpdate()
    {

        if (seguir && personaje != null)
        {
            transform.position = new Vector3(personaje.position.x, personaje.position.y, -10f);
        }
    }
}
