using DG.Tweening;
using UnityEngine;


public class NoCoinAnimation : MonoBehaviour
{
    public float slideUpDistance = 80f;
    public float slideDuration = 0.5f;
    public float holdDuration = 0.8f;
    public float fadeDuration = 0.3f;

    private Sequence _currentSeq;

    public void Play(GameObject targetImage)
    {
        if (_currentSeq != null && _currentSeq.IsActive())
        {
            _currentSeq.Kill(false);
            var oldGroup = targetImage.GetComponent<CanvasGroup>();
            if (oldGroup != null) oldGroup.alpha = 1f;
            targetImage.SetActive(false);
        }

        var rect = targetImage.GetComponent<RectTransform>();
        var group = targetImage.GetComponent<CanvasGroup>();
        if (group == null) group = targetImage.AddComponent<CanvasGroup>();

        var originPos = rect.anchoredPosition;

        targetImage.SetActive(true);
        group.alpha = 1f;
        rect.anchoredPosition = new Vector2(originPos.x, originPos.y - slideUpDistance);

        _currentSeq = DOTween.Sequence();
        _currentSeq.Append(rect.DOAnchorPosY(originPos.y, slideDuration).SetEase(Ease.OutBack));
        _currentSeq.AppendInterval(holdDuration);
        _currentSeq.Append(group.DOFade(0f, fadeDuration));
        _currentSeq.OnComplete(() =>
        {
            if (targetImage == null) return;
            targetImage.SetActive(false);
            rect.anchoredPosition = originPos;
            group.alpha = 1f;
            _currentSeq = null;
        });
    }
}
