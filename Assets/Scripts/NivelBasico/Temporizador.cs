using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Temporizador : MonoBehaviour
{
    public Text textoTemporizador; // Texto que muestra el tiempo
    public Image barraTiempo; // Imagen de la barra azul
    public float tiempoTotal = 120f; // Duraci�n en segundos (2 minutos)
    private float tiempoRestante;

    public AudioSource audioIntro; // Arrastra aquí el AudioSource del audio de la interfaz
    private bool temporizadorActivo = false;

    private void Start()
    {
        tiempoRestante = tiempoTotal;
        ActualizarTexto();

        if (audioIntro != null)
        {
            audioIntro.Play();
            temporizadorActivo = false;
            Invoke(nameof(IniciarTemporizador), audioIntro.clip.length);
        }
        else
        {
            temporizadorActivo = true; // Si no hay audio, inicia de inmediato
        }
    }

    private void IniciarTemporizador()
    {
        temporizadorActivo = true;
    }

    private void Update()
    {
        if (!temporizadorActivo) return;

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
            barraTiempo.rectTransform.localScale = new Vector3(porcentaje, 1, 1); // Ajusta el tama�o horizontalmente
        }
    }
}
