using UnityEngine;
using UnityEngine.UI; // Necesario para Button

public class HudBtnController : MonoBehaviour
{
    public GameObject HUD; // Arrastras aqu� tu HUD (el panel azul que quieres ocultar)

    //private Button boton; // Referencia al bot�n

    /*void Start()
    {
        boton = GetComponent<Button>();

        if (boton != null)
        {
            boton.onClick.AddListener(OcultarHUD);
        }
        else
        {
            Debug.LogError("No se encontr� el componente Button en " + gameObject.name);
        }
    }*/

    public void OcultarHUD()
    {
        HUD.SetActive(false);
    }
}
