using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Temporizador : MonoBehaviour
{
    public Text textoTemporizador; // Texto que muestra el tiempo
    public Image barraTiempo; // Imagen de la barra azul
    public float tiempoTotal = 120f; // Duración en segundos (2 minutos)
    private float tiempoRestante;

    private void Start()
    {
        tiempoRestante = tiempoTotal;
        ActualizarTexto();
    }

    private void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarTexto();
            ActualizarBarra();
        }
        else
        {
            tiempoRestante = 0;
        }
    }

    private void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTemporizador.text = minutos.ToString("0") + ":" + segundos.ToString("00") + " MINUTOS";
    }

    private void ActualizarBarra()
    {
        if (barraTiempo != null)
        {
            float porcentaje = tiempoRestante / tiempoTotal;
            barraTiempo.rectTransform.localScale = new Vector3(porcentaje, 1, 1); // Ajusta el tamaño horizontalmente
        }
    }
}
