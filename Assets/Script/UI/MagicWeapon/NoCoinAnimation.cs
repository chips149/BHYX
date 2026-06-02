using DG.Tweening;
using UnityEngine;


public class NoCoinAnimation : MonoBehaviour
{
    public float slideUpDistance = 80f;
    public float slideDuration = 0.5f;
    public float holdDuration = 0.8f;
    public float fadeDuration = 0.3f;

    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Vector2 originPos;

    public void Play(GameObject targetImage)
    {
        rect = targetImage.GetComponent<RectTransform>();
        canvasGroup = targetImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = targetImage.AddComponent<CanvasGroup>();

        originPos = rect.anchoredPosition;

        targetImage.SetActive(true);
        canvasGroup.alpha = 1f;
        rect.anchoredPosition = new Vector2(originPos.x, originPos.y - slideUpDistance);

        Sequence seq = DOTween.Sequence();
        seq.Append(rect.DOAnchorPosY(originPos.y, slideDuration).SetEase(Ease.OutBack));
        seq.AppendInterval(holdDuration);
        seq.Append(canvasGroup.DOFade(0f, fadeDuration));
        seq.OnComplete(() =>
        {
            targetImage.SetActive(false);
            rect.anchoredPosition = originPos;
            canvasGroup.alpha = 1f;
        });
    }
}
