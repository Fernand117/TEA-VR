using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobyController : MonoBehaviour
{
    public void LoadLevelIndex(int level)
    {
        SceneManager.LoadScene(level);
    }
}
