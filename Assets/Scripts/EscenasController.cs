using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenasController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cargarEscenas(string nombre)
    {
        Debug.Log("Cambiando a la escena: " + nombre);
        SceneManager.LoadScene(nombre);
    }
}
