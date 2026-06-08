using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBootClicked()
    {
        SceneManager.LoadScene("BootScene");
    }
}

