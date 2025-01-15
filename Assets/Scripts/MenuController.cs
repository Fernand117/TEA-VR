using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con elementos UI como Button

public class MenuController : MonoBehaviour
{
    public GameObject menuPrefab; // Prefab del men� de opciones
    private GameObject instantiatedMenu;

    public Button btnRegresar; // Referencia al bot�n "Regresar"
    public Button btnSalir;    // Referencia al bot�n "Salir"

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

        // Encuentra los botones dentro del men� instanciado
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
        // Acci�n cuando se hace clic en el bot�n "Regresar"
        Debug.Log("Bot�n Regresar presionado");
        // Aqu� puedes a�adir la l�gica para regresar al juego o cambiar de escena
        ToggleMenu(); // Por ejemplo, cerrando el men�
    }

    void OnSalirClicked()
    {
        // Acci�n cuando se hace clic en el bot�n "Salir"
        Debug.Log("Bot�n Salir presionado");
        // Aqu� puedes a�adir la l�gica para salir del juego
        Application.Quit();
    }
}
