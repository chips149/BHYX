using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
}
