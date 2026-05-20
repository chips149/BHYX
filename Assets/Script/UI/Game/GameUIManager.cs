using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    public GameObject losePanel;
    public GameObject winPanel;
    public Text winText;
    public Text loseText;

    private void Awake()
    {
        instance = this;
    }

    public void Win()
    {
        winPanel.SetActive(true);
        GameState.isGameOver = true;
        winText.text = $"获得火晶:{CoinSystem.sessionCoin}";
    }
    public void Lose()
    {
        losePanel.SetActive(true);
        GameState.isGameOver = true;
        loseText.text=$"获得火晶:{CoinSystem.sessionCoin}";
    }
    
    public void OnBootClicked()
    {
        GameState.isGameOver = false;
        SceneManager.LoadScene("BootScene");
    }

    public void OnMainMenuClicked()
    {
        GameState.isGameOver = false;
        SceneManager.LoadScene("MainMenuScene");
    }
}
