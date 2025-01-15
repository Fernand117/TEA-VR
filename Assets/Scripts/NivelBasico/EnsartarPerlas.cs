using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnsartarPerlas : MonoBehaviour
{
    public Text txtContador;
    private int contador = 0;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión con Perla detectada");
        if (collision.collider.CompareTag("Perla"))
        {
            contador++;
            txtContador.text = contador.ToString();
        }
    }
}
