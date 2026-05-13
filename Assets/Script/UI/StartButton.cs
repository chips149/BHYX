using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
