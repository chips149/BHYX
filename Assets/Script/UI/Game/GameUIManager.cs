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

    private void Awake()
    {
        instance = this;
    }

    public void Lose()
    {
        losePanel.SetActive(true);
        GameState.isGameOver = true;
    }
    
    public void OnMainMenuClicked()
    {
        GameState.isGameOver = false;
        Debug.Log("点击按钮");
        SceneManager.LoadScene("MainMenuScene");
    }
}
