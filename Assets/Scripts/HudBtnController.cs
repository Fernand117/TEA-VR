using UnityEngine;
using UnityEngine.UI; // Necesario para Button

public class HudBtnController : MonoBehaviour
{
    public GameObject HUD; // Arrastras aquí tu HUD (el panel azul que quieres ocultar)

    //private Button boton; // Referencia al botón

    /*void Start()
    {
        boton = GetComponent<Button>();

        if (boton != null)
        {
            boton.onClick.AddListener(OcultarHUD);
        }
        else
        {
            Debug.LogError("No se encontró el componente Button en " + gameObject.name);
        }
    }*/

    public void OcultarHUD()
    {
        HUD.SetActive(false);
    }
}
