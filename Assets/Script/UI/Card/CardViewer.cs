
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class CardViewer : MonoBehaviour
{
    private DrawCardPanel _drawCardPanel;
    private int _index;
    private CardData _cardData;
    public Image img;
    public Text nameText;
    public Text description;
    public Text detailText;
    public RectTransform cardRoot;

    [Header("卡牌背景")]
    public string propertyBgPath = "UI/Card/UI_HUD_BasePlate_IMG(Blue)";
    public string mechanismBgPath = "UI/Card/UI_HUD_BasePlate_IMG(Purple)";

    private Button _btn;
    private bool _isShowingDetail;
    private bool _isFlipping;
    private const float FlipDuration = 0.35f;
    public void Initialize(DrawCardPanel drawCardPanel, int index, CardData data)
    {
        _drawCardPanel = drawCardPanel;
        _index = index;
        _cardData = data;
        
        Sprite sprite = Resources.Load<Sprite>(_cardData.imgPath);
        img.sprite = sprite;
        nameText.text = _cardData.name;
        description.text = _cardData.description;
        detailText.text = _cardData.detailText;
        _isShowingDetail = false;
        _isFlipping = false;
        cardRoot.localScale = Vector3.one;
        img.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        detailText.gameObject.SetActive(false);

        string bgPath = _cardData.isSkillCard ? mechanismBgPath : propertyBgPath;
        var bgSprite = Resources.Load<Sprite>(bgPath);
        if (bgSprite != null) GetComponent<Image>().sprite = bgSprite;

        _btn = GetComponent<Button>();
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        _drawCardPanel.OnCardSelected(_index, _cardData);
    }
    public void SetSelected(bool selected)
    {
        if (selected)
        {
            cardRoot.DOScale(1.1f, 0.23f).SetEase(Ease.OutBack);
        }
        else
        {
            cardRoot.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
        }
    }
    public void OnDetailButtonClicked()
    {
        if (_isFlipping) return;
        _isFlipping = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(cardRoot.DOScaleX(0f, FlipDuration * 0.5f).SetEase(Ease.InQuad));
        sequence.AppendCallback(SwitchCardFace);
        sequence.Append(cardRoot.DOScaleX(1f, FlipDuration * 0.5f).SetEase(Ease.OutQuad));
        sequence.OnComplete(() => _isFlipping = false);
    }
    private void SwitchCardFace()
    {
        _isShowingDetail = !_isShowingDetail;
        if (_isShowingDetail)
        {
            img.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
            detailText.text = _cardData.detailText;
            detailText.gameObject.SetActive(true);
        }
        else
        {
            img.gameObject.SetActive(true);
            description.gameObject.SetActive(true);
            detailText.gameObject.SetActive(false);
        }
    }
}

