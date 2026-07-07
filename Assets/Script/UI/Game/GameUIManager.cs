using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    public GameObject losePanel;
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    [Header("卡牌结算")]
    [SerializeField] private CardSettlementUI cardSettlementUI;

    [Header("设置面板")]
    public GameObject settingPanel;

    [Header("详情面板")]
    public GameObject detailPanel;

    [Header("返回确认面板")]
    public GameObject returnPanel;

    [Header("新手教程")]
    public GameObject tutorialRoot;
    public Image[] tutorialImages;
    private int _tutorialIndex;

    [Header("音效控制")]
    public Button bgmToggleButton;
    public Button sfxToggleButton;
    public Sprite bgmOnIcon;
    public Sprite bgmOffIcon;
    public Sprite sfxOnIcon;
    public Sprite sfxOffIcon;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
        if (detailPanel != null) detailPanel.SetActive(false);
        if (returnPanel != null) returnPanel.SetActive(false);
        if (tutorialRoot != null) tutorialRoot.SetActive(false);
        Time.timeScale = 1f;

        UpdateSoundIcons();
        ShowTutorialIfNeeded();
    }

    private IEnumerator DelayedUnpause()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 1f;
    }

    private void ShowTutorialIfNeeded()
    {
        int playCount = PlayerPrefs.GetInt("GamePlayCount", 0);
        bool alreadyShown = PlayerPrefs.GetInt("TutorialShown_" + playCount, 0) == 1;

        if (playCount < 3 && !alreadyShown && tutorialRoot != null && tutorialImages.Length > 0)
        {
            PlayerPrefs.SetInt("TutorialShown_" + playCount, 1);
            PlayerPrefs.Save();

            _tutorialIndex = 0;
            tutorialRoot.SetActive(true);
            Time.timeScale = 0f;
            ShowTutorialImage(0);
        }

        PlayerPrefs.SetInt("GamePlayCount", playCount + 1);
        PlayerPrefs.Save();
    }

    private void ShowTutorialImage(int index)
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            if (tutorialImages[i] != null)
                tutorialImages[i].gameObject.SetActive(i == index);
        }
        _tutorialIndex = index;
    }

    public void OnTutorialNextClicked()
    {
        if (_tutorialIndex + 1 < tutorialImages.Length)
            ShowTutorialImage(_tutorialIndex + 1);
    }

    public void OnTutorialCloseClicked()
    {
        if (tutorialRoot != null) tutorialRoot.SetActive(false);
        StartCoroutine(DelayedUnpause());
    }

    public void OnSettingClicked()
    {
        if (settingPanel == null) return;
        settingPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnCloseSettingClicked()
    {
        if (settingPanel == null) return;
        settingPanel.SetActive(false);
        StartCoroutine(DelayedUnpause());
    }

    public void OnDetailClicked()
    {
        if (detailPanel == null) return;
        detailPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnCloseDetailClicked()
    {
        if (detailPanel == null) return;
        detailPanel.SetActive(false);
        StartCoroutine(DelayedUnpause());
    }

    public void OnReturnClicked()
    {
        if (returnPanel == null) return;
        returnPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnCancelReturnClicked()
    {
        if (returnPanel == null) return;
        returnPanel.SetActive(false);
        StartCoroutine(DelayedUnpause());
    }

    public void OnConfirmReturnClicked()
    {
        OnMainMenuClicked();
    }

    private void UpdateSoundIcons()
    {
        SetButtonIcon(bgmToggleButton, SoundManager.IsBgmMuted ? bgmOffIcon : bgmOnIcon);
        SetButtonIcon(sfxToggleButton, SoundManager.IsSfxMuted ? sfxOffIcon : sfxOnIcon);
    }

    private void SetButtonIcon(Button btn, Sprite sprite)
    {
        if (btn != null && sprite != null)
        {
            var img = btn.GetComponent<Image>();
            if (img != null) img.sprite = sprite;
        }
    }

    public void OnToggleBgmClicked()
    {
        SoundManager.ToggleBgmMute();
        SetButtonIcon(bgmToggleButton, SoundManager.IsBgmMuted ? bgmOffIcon : bgmOnIcon);
    }

    public void OnToggleSfxClicked()
    {
        SoundManager.ToggleSfxMute();
        SetButtonIcon(sfxToggleButton, SoundManager.IsSfxMuted ? sfxOffIcon : sfxOnIcon);
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
        loseText.text = $"获得火晶:{CoinSystem.sessionCoin}";

        ShowCardSettlement();
    }

    private void ShowCardSettlement()
    {
        if (cardSettlementUI != null)
        {
            cardSettlementUI.Show();
        }
        else
        {
            var found = FindObjectOfType<CardSettlementUI>(true);
            if (found != null) found.Show();
        }
    }
    
    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        GameState.isGameOver = false;
        CoinSystem.CommitSessionCoins();
        SaveManager.ClearPersistedSaveForNewGame();
        SceneManager.LoadScene("MainMenuScene");
    }
}
