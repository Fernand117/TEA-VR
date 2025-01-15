using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con elementos UI como Button

public class MenuController : MonoBehaviour
{
    public GameObject menuPrefab; // Prefab del menú de opciones
    private GameObject instantiatedMenu;

    public Button btnRegresar; // Referencia al botón "Regresar"
    public Button btnSalir;    // Referencia al botón "Salir"

    private static MenuController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InstantiateMenu();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Asignar las funciones a los botones
        btnRegresar.onClick.AddListener(OnRegresarClicked);
        btnSalir.onClick.AddListener(OnSalirClicked);
    }

    void InstantiateMenu()
    {
        instantiatedMenu = Instantiate(menuPrefab);
        instantiatedMenu.SetActive(false);

        // Encuentra los botones dentro del menú instanciado
        btnRegresar = instantiatedMenu.transform.Find("BackgroundMenu/btnRegresar").GetComponent<Button>();
        btnSalir = instantiatedMenu.transform.Find("BackgroundMenu/btnSalir").GetComponent<Button>();
    }

    void ToggleMenu()
    {
        bool isActive = instantiatedMenu.activeSelf;
        instantiatedMenu.SetActive(!isActive);
    }

    void OnRegresarClicked()
    {
        // Acción cuando se hace clic en el botón "Regresar"
        Debug.Log("Botón Regresar presionado");
        // Aquí puedes añadir la lógica para regresar al juego o cambiar de escena
        ToggleMenu(); // Por ejemplo, cerrando el menú
    }

    void OnSalirClicked()
    {
        // Acción cuando se hace clic en el botón "Salir"
        Debug.Log("Botón Salir presionado");
        // Aquí puedes añadir la lógica para salir del juego
        Application.Quit();
    }
}
