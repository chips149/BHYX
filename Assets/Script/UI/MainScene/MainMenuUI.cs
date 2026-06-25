using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("音效控制")]
    public Button soundToggleButton;
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;

    private void Start()
    {
        SoundManager.PlayBGM("Audio/BGM/MainMenu");
        UpdateSoundIcon();
    }

    private void UpdateSoundIcon()
    {
        if (soundToggleButton != null)
        {
            var img = soundToggleButton.GetComponent<Image>();
                img.sprite = SoundManager.IsMuted ? soundOffIcon : soundOnIcon;
        }
    }

    public void OnToggleSoundClicked()
    {
        SoundManager.ToggleMute();
        UpdateSoundIcon();
    }

    public void OnStartGameClicked()
    {
        if (SaveManager.HasSaveFile())
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            SaveManager.ClearPersistedSaveForNewGame();
            SceneManager.LoadScene("GameScene");
        }
    }

    public void OnBootClicked()
    {
        SceneManager.LoadScene("BootScene");
    }

    public void OnDetailConfirmClicked()
    {
        SceneManager.LoadScene("BootScene");
    }
}

