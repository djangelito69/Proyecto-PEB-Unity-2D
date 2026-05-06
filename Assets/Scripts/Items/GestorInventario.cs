using UnityEngine;

public class GestorInventario : MonoBehaviour
{
    [SerializeField] private KeyCode teclaAbrir = KeyCode.I;
    [SerializeField] private PanelInventario panelInventario;

    void Update()
    {
        if (Input.GetKeyDown(teclaAbrir))
        {
            if (panelInventario.EstaAbierto)
                panelInventario.Cerrar();
            else
                panelInventario.Abrir();
        }
    }
}