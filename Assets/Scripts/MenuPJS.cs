using UnityEngine;

public class MenuPJS : MonoBehaviour
{
    public GameObject panel;

    public void ToggleMenu()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
