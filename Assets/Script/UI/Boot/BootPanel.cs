using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootPanel : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.gameObject.SetActive(SaveManager.HasSaveFile());
    }

    public void OnContinueClicked()
    {
        if (!SaveManager.HasSaveFile())
            return;

        SceneManager.LoadScene("GameScene");
    }

    public void OnNewGameClicked()
    {
        SaveManager.ClearPersistedSaveForNewGame();
        SceneManager.LoadScene("MainMenuScene");
    }
}
