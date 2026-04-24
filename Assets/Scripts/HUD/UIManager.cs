using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelInventario;
    public GameObject panelOpciones;

    private bool juegoPausado = false;
    private bool musicaSilenciada = false;

    public void ToggleInventario()
    {
        panelInventario.SetActive(
            !panelInventario.activeSelf
        );
    }

    public void TogglePausa()
    {
        juegoPausado = !juegoPausado;

        Time.timeScale = juegoPausado ? 0f : 1f;

        AudioListener.pause = juegoPausado;
    }

    public void ToggleMusica()
    {
        musicaSilenciada = !musicaSilenciada;

        AudioListener.volume =
            musicaSilenciada ? 0f : 1f;
    }
    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
    }

    public void ToggleOpciones()
    {
        panelOpciones.SetActive(
            !panelOpciones.activeSelf
        );
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);
    }

    public void IrAMenuPrincipal()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuPrincipal");
    }
}