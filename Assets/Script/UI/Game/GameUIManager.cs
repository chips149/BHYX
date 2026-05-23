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

    [Header("卡牌结算")]
    [SerializeField] private CardSettlementUI cardSettlementUI;

    private void Awake()
    {
        instance = this;
    }

    public void Win()
    {
        winPanel.SetActive(true);
        GameState.isGameOver = true;
        winText.text = $"获得火晶:{CoinSystem.sessionCoin}";

        ShowCardSettlement();
    }
    public void Lose()
    {
        losePanel.SetActive(true);
        GameState.isGameOver = true;
        loseText.text=$"获得火晶:{CoinSystem.sessionCoin}";

        ShowCardSettlement();
    }

    private void ShowCardSettlement()
    {
        if (cardSettlementUI != null)
            cardSettlementUI.Show();
        else
            FindObjectOfType<CardSettlementUI>(true)?.Show();
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
