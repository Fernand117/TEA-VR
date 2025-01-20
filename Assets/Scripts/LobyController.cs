using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobyController : MonoBehaviour
{
    public Button btnPerlas;
    public Button btnBebidas;
    public Button btnClips;

    // Start is called before the first frame update
    void Start()
    {
        btnPerlas.onClick.AddListener(OnPerlasClick);
        btnBebidas.onClick.AddListener(OnBebidasClick);
        btnClips.onClick.AddListener(OnClipsClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadLevelIndex(int level)
    {
        SceneManager.LoadScene(level);
    }

    void OnPerlasClick()
    {
        LoadLevelIndex(0);
    }

    void OnBebidasClick()
    {
        LoadLevelIndex(1);
    }

    void OnClipsClick()
    {
        LoadLevelIndex(2);
    }
}
