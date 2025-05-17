using UnityEngine;
using UnityEngine.UI;

public class HudBtnController : MonoBehaviour
{
    public GameObject HUD;
    public AudioSource audioInstrucciones; // Referencia al AudioSource

    void Start()
    {
        // Verifica si hay un AudioSource asignado
        if (audioInstrucciones != null)
        {
            // Calcula la duración del audio y programa la ocultación del HUD
            Invoke("OcultarHUD", audioInstrucciones.clip.length);
        }
    }

    public void OcultarHUD()
    {
        HUD.SetActive(false);
    }
}
