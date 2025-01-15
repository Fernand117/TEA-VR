using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaController : MonoBehaviour
{
    private bool puertaAbierta = false; 
    public float anguloApertura = 90f;  
    public float velocidadApertura = 2f; 

    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;

    void Start()
    {
        rotacionInicial = transform.rotation;
        rotacionFinal = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));
    }

    void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

            if (hit.transform == transform)
            {
                Debug.Log("Clic en la puerta detectado.");
                puertaAbierta = !puertaAbierta;
                StopAllCoroutines();
                StartCoroutine(MoverPuerta());
            }
        }
        else
        {
            Debug.Log("Raycast no golpeó nada.");
        }
    }
}

IEnumerator MoverPuerta()
{
    Quaternion rotacionObjetivo = puertaAbierta ? rotacionFinal : rotacionInicial;
    Debug.Log("Rotación objetivo: " + rotacionObjetivo.eulerAngles);

    while (Quaternion.Angle(transform.rotation, rotacionObjetivo) > 0.01f)
    {
        Debug.Log("Rotación actual: " + transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadApertura);
        yield return null;
    }

    transform.rotation = rotacionObjetivo;
    Debug.Log("Puerta rotada a: " + transform.rotation.eulerAngles);
}

}
