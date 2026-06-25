using UnityEngine;

public class BootLoader : MonoBehaviour
{
    public GameObject privacyPanel;

    private void Start()
    {
        if (privacyPanel != null) privacyPanel.SetActive(true);
        SoundManager.PlayBGM("Audio/BGM/BootScence");
    }

    public void OnAgreeClicked()
    {
        if (privacyPanel != null) privacyPanel.SetActive(false);
    }

    public void OnRejectClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
