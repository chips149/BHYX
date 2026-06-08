using UnityEngine;
using UnityEngine.SceneManagement;

public class BootPanel : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
