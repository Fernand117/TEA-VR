using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorController : MonoBehaviour
{
    private Rigidbody rb;

    [Range(1, 10)]
    public float velocidadMovimiento = 5f;

    [Range(1, 100)]
    public float velocidadRotacion = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   void FixedUpdate()
    {
        // Movimiento hacia adelante y hacia atrás
        float movV = Input.GetAxis("Vertical") * velocidadMovimiento * Time.deltaTime;

        // Rotación izquierda y derecha
        float rotH = Input.GetAxis("Horizontal") * velocidadRotacion * Time.deltaTime;

        // Movimiento en la dirección en la que está mirando el personaje
        Vector3 movimiento = transform.forward * movV;
        rb.MovePosition(rb.position + movimiento);

        // Rotar el objeto alrededor del eje Y
        if (rotH != 0)
        {
            Quaternion rotacion = Quaternion.Euler(0.0f, rotH, 0.0f);
            rb.MoveRotation(rb.rotation * rotacion);
        }
    }
}
