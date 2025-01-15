using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nivel1Controller : MonoBehaviour
{
    public float tiempoInicial = 60f; 
    private float tiempoRestante;
    public TextMeshProUGUI textoTiempo;  
    public GameObject menuPausa;  
    public GameObject panelInstrucciones;  

    private Coroutine temporizadorCoroutine;
    private bool estaPausado = false;

    void Start()
    {
        panelInstrucciones.SetActive(true);
        menuPausa.SetActive(false);
        tiempoRestante = tiempoInicial;
        MostrarTiempo(tiempoRestante);
    }

    public void ComenzarJuego()
    {
        panelInstrucciones.SetActive(false);
        ReiniciarTemporizador();
    }

    public void ReiniciarTemporizador()
    {
        if (temporizadorCoroutine != null)
        {
            StopCoroutine(temporizadorCoroutine);
        }

        tiempoRestante = tiempoInicial;
        estaPausado = false;
        menuPausa.SetActive(false);  // Ocultar el menú de pausa si está activo
        temporizadorCoroutine = StartCoroutine(TemporizadorCoroutine());
    }

    public void PausarTemporizador()
    {
        estaPausado = true;
        menuPausa.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ReanudarTemporizador()
    {
        estaPausado = false;
        menuPausa.SetActive(false);  
        Time.timeScale = 1f;  
    }

    IEnumerator TemporizadorCoroutine()
    {
        while (tiempoRestante > 0)
        {
            if (!estaPausado)
            {
                tiempoRestante -= Time.deltaTime;
                MostrarTiempo(tiempoRestante);
            }

            yield return null;
        }

        Debug.Log("Tiempo finalizado");
    }

    void MostrarTiempo(float tiempoParaMostrar)
    {
        float minutos = Mathf.FloorToInt(tiempoParaMostrar / 60);
        float segundos = Mathf.FloorToInt(tiempoParaMostrar % 60);
        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);  // Actualizar el texto en el TextMeshPro
    }
}
