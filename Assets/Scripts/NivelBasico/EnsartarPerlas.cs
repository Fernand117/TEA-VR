using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnsartarPerlas : MonoBehaviour
{
    public Text txtContador;
    private int contador = 0;
    public AudioClip checkPerla;
    public AudioSource audioSource;

    public List<Image> circulosIndicadores; // arrastra aquí los 5 circulitos en el inspector
    public Sprite circuloActivo;             // arrastra aquí el nuevo sprite (imagen activa)

    // Referencias al canvas de felicitación y textos
    public GameObject canvasFelicitaciones;
    public Text txtTotalAciertos;
    public Text txtTiempo;
    public Text txtTotalIncorrectos;

    // Variables para el tiempo
    private float tiempoTranscurrido = 0f;
    private bool juegoActivo = true;

    private void Start()
    {
        // Asegurarse que el canvas esté desactivado al inicio
        if (canvasFelicitaciones != null)
        {
            canvasFelicitaciones.SetActive(false);
        }
    }

    // Agregar variable para el límite de tiempo
    private const float TIEMPO_LIMITE = 120f; // 2 minutos en segundos

    private void Update()
    {
        if (juegoActivo)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            // Verificar si se acabó el tiempo
            if (tiempoTranscurrido >= TIEMPO_LIMITE)
            {
                MostrarPanelFelicitaciones();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Agregar esta verificación al inicio
        if (!juegoActivo || contador >= 5) return;

        Debug.Log("Colisi�n detectada con: " + collision.GetComponent<Collider>().name);

        if (collision.GetComponent<Collider>().CompareTag("Perla"))
        {
            audioSource.clip = checkPerla;
            audioSource.Play();

            Transform perlaTransform = collision.transform;

            // Fijar la Perla como hija de la C�psula
            perlaTransform.SetParent(transform, true); // Se mantiene en su posici�n global

            // Desactivar la f�sica para que no se mueva m�s
            Rigidbody rb = perlaTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Evitar que la f�sica lo siga afectando
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero; // Evitar que se muevan despu�s de pegarse
                rb.angularVelocity = Vector3.zero; // Evitar giros indeseados
            }

            // 🚨 IMPORTANTE: Desactivar el collider
            Collider col = collision.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            // Incrementar el contador
            contador++;
            txtContador.text = contador.ToString();
            Debug.Log("Contador actualizado a: " + contador);

            // Cambiar la imagen del círculo correspondiente
            if (contador - 1 < circulosIndicadores.Count)
            {
                circulosIndicadores[contador - 1].sprite = circuloActivo;
            }

            // Verificar si se completaron las 5 perlas
            if (contador >= 5)
            {
                MostrarPanelFelicitaciones();
            }
        }
    }

    private void MostrarPanelFelicitaciones()
    {
        juegoActivo = false;

        // Calcular estadísticas
        int totalAciertos = contador;
        int totalIncorrectos = 5 - contador; // Los que faltaron para completar 5
        float tiempoMinutos = tiempoTranscurrido / 60f;

        // Actualizar textos
        if (txtTotalAciertos != null)
            txtTotalAciertos.text = totalAciertos.ToString();
        
        if (txtTiempo != null)
            txtTiempo.text = tiempoMinutos.ToString("F1") + " minutos";
        
        if (txtTotalIncorrectos != null)
            txtTotalIncorrectos.text = totalIncorrectos.ToString();

        // Mostrar el canvas
        if (canvasFelicitaciones != null)
        {
            canvasFelicitaciones.SetActive(true);
        }
    }
}
