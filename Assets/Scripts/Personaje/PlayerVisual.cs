using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator animator;

    [Header("Sprites por personaje")]
    public Sprite gato;
    public Sprite perro;
    public Sprite raton;

    public RuntimeAnimatorController gatoAnim;
    public RuntimeAnimatorController perroAnim;
    public RuntimeAnimatorController ratonAnim;

    void Start()
    {
        AplicarPersonaje();
    }

    public void AplicarPersonaje()
    {
        var tipo = DatosPersonaje.PersonajeSeleccionado;

        switch (tipo)
        {
            case DatosPersonaje.TipoPersonaje.Gato:
                sr.sprite = gato;
                animator.runtimeAnimatorController = gatoAnim;
                break;

            case DatosPersonaje.TipoPersonaje.Perro:
                sr.sprite = perro;
                animator.runtimeAnimatorController = perroAnim;
                break;

            case DatosPersonaje.TipoPersonaje.Raton:
                sr.sprite = raton;
                animator.runtimeAnimatorController = ratonAnim;
                break;
        }
    }
}