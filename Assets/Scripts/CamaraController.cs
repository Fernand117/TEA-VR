using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset; 

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - jugador.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = jugador.position + jugador.TransformDirection(offset);

        transform.LookAt(jugador);
    }
}
