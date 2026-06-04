using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectModePanel : MonoBehaviour
{
    [Serializable]
    public class ModeConfig
    {
        public string modeName;
        public int startLevel = 1;
        public bool isEndless;
    }

    
    private int currentIndex;
    private bool isAnimating;

    public ModeConfig[] modes;
    public Text modeText;
    public Button leftButton;
    public Button rightButton;
    public RectTransform textContainer;
    
    private float slideDuration = 0.1f;
    private float slideOffset = 150f;

    private void Start()
    {
        ApplyMode(0); 
    }
    
    public void OnPrevClicked()
    {
        if (isAnimating) return;
        int prev = (currentIndex - 1 + modes.Length) % modes.Length;
        SwitchTo(prev, 1);
    }
    
    public void OnNextClicked()
    {
        if (isAnimating) return;
        int next = (currentIndex + 1) % modes.Length;
        SwitchTo(next, -1);
    }

    private void SwitchTo(int newIndex, int direction)
    {
        if (newIndex == currentIndex) return;

        isAnimating = true;
        var parent = modeText.transform.parent;
        var oldText = modeText;

        var newTextObj = Instantiate(oldText.gameObject, parent);
        newTextObj.transform.SetSiblingIndex(oldText.transform.GetSiblingIndex());
        var newText = newTextObj.GetComponent<Text>();
        var config = modes[newIndex];

        newText.text = config.modeName;
        Vector2 oldPos = oldText.rectTransform.anchoredPosition;
        newText.rectTransform.anchoredPosition = oldPos + new Vector2(slideOffset * direction, 0);

        Sequence seq = DOTween.Sequence();
        seq.Join(oldText.rectTransform.DOAnchorPosX(oldPos.x - slideOffset * direction, slideDuration));
        seq.Join(oldText.DOFade(0f, slideDuration * 0.5f)); 
        seq.Join(newText.rectTransform.DOAnchorPosX(oldPos.x, slideDuration));
        seq.SetEase(Ease.OutQuad);
        seq.OnComplete(() =>
        {
            Destroy(oldText.gameObject);
            modeText = newText;
            isAnimating = false;
        });

        currentIndex = newIndex;
        ApplyMode(newIndex);
    }

    private void ApplyMode(int index)
    {
        var config = modes[index];
        GameState.isEndlessMode = config.isEndless;
        GameState.currentLevel = config.startLevel;
    }
}
