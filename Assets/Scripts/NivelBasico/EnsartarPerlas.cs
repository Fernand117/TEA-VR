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


    private void OnTriggerEnter(Collider collision)
    {
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
            if (contador - 1 < circulosIndicadores.Count) // recuerda que las listas son base 0
            {
                circulosIndicadores[contador - 1].sprite = circuloActivo;
            }
        }
    }


}
