using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        if (SaveManager.HasSaveFile())
        {
            // 有存档 -> 继续游戏（GameInitializer.Start 会调 SaveManager.Load）
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // 无存档 -> 新游戏，清除旧数据
            SaveManager.ClearPersistedSaveForNewGame();
            SceneManager.LoadScene("GameScene");
        }
    }

    public void OnBootClicked()
    {
        SceneManager.LoadScene("BootScene");
    }
}

