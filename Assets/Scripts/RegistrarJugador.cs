using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RegistrarJugador : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public TMP_InputField inputGenero;
    public TMP_InputField inputEdad;

    public void Registrar()
    {
        // Si no existe el singleton, créalo
        if (JugadorActual.Instance == null)
        {
            GameObject go = new GameObject("JugadorActual");
            go.AddComponent<JugadorActual>();
        }
        JugadorActual.Instance.Nombre = inputNombre.text;
        JugadorActual.Instance.Genero = inputGenero.text;
        JugadorActual.Instance.Edad = inputEdad.text;

        // Reconfigurar rutas de guardado para que use el nombre correcto
        if (ResultadosManager.Instance != null)
        {
            ResultadosManager.Instance.ConfigurarRutasGuardado();
        }

        SceneManager.LoadScene(6); // Cambia al nivel 1 (Loby)
    }
}

// Clase singleton persistente
public class JugadorActual : MonoBehaviour
{
    public static JugadorActual Instance;
    public string Nombre;
    public string Genero;
    public string Edad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
