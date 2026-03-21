using UnityEngine;

public class Inventario : MonoBehaviour
{
    public GameObject panel;

    public void ToggleMenu()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
