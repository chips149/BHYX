using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Text CoinText;

    public void OnEnable()
    {
        CoinText.text = $"火晶：{CoinSystem.GetCoin()}";
    }

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBootClicked()
    {
        SceneManager.LoadScene("BootScene");
    }
}

