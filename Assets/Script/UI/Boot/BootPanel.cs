using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootPanel : MonoBehaviour
{

    public void OnNewGameClicked()
    {
        SaveManager.ClearPersistedSaveForNewGame();
        SceneManager.LoadScene("MainMenuScene");
    }
}
