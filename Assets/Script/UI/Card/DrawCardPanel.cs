using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DrawCardPanel : MonoBehaviour
{
    private CardViewer[] _viewers;
    [SerializeField] private Button refreshButton;

    [Header("已获得卡牌显示")]
    public Transform haveCardContainer;
    public GameObject cardIconPrefab;

    [Header("法宝属性面板")]
    public AttributesPanel weaponPropertyPanel;

    [Header("旋转装饰图")]
    public Image rotatingImage;

    [Header("关卡完成提示")]
    public Text levelCompleteText;

    private int _selectedIndex = -1;
    private CardData _selectedCardData;
    private Tween _rotateTween;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

        if (rotatingImage != null)
        {
            _rotateTween?.Kill();
            _rotateTween = rotatingImage.transform
                .DOLocalRotate(new Vector3(0, 0, -360), 6f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }

    private void OnDisable()
    {
        _rotateTween?.Kill();
    }

    void RandomCard()
    {
        _viewers ??= transform.GetComponentsInChildren<CardViewer>();
        var data = CardHandler.RandomCardData();
        for (var i = 0; i < _viewers.Length && i < data.Length; i++)
        {
            _viewers[i].Initialize(this, i, data[i]);
        }

        _selectedIndex = -1;
        _selectedCardData = null;
    }

    public void OnCardSelected(int index, CardData cardData)
    {
        _selectedIndex = index;
        _selectedCardData = cardData;

        for (int i = 0; i < _viewers.Length; i++)
        {
            _viewers[i].SetSelected(i == index);
        }
    }

    public void ConfirmSelection()
    {
        if (_selectedCardData == null) return;
        SaveData.AddCard(_selectedCardData.id);
        _selectedCardData.OnChosen();
        RefreshHaveCards();
        CloseDrawCardPanel();
    }

    public void OpenDrawCardPanel()
    {
        if (this == null || gameObject == null) return;
        gameObject.SetActive(true);
        RandomCard();
        RefreshHaveCards();
        weaponPropertyPanel?.RefreshPanel();

        if (levelCompleteText != null)
            levelCompleteText.text = $"第{GameState.currentLevel}关已完成";
    }

    public void CloseDrawCardPanel()
    {
        gameObject.SetActive(false);
        GameState.currentLevel++;

        EnvironmentManager.Instance.CheckAndSwitch(GameState.currentLevel);

        SaveManager.ToSave();
        SpawnMonsterHandler.Instance.StartSpawn();
    }

    public void OnRefreshClicked()
    {
        RandomCard();
    }

    private void RefreshHaveCards()
    {
        foreach (Transform child in haveCardContainer)
            Destroy(child.gameObject);

        foreach (int cardId in SaveData.Instance.chosenCardIds)
        {
            var cardData = CardHandler.Data.FirstOrDefault(c => c.id == cardId);
            if (cardData == null) continue;

            var icon = Instantiate(cardIconPrefab, haveCardContainer);
            var img = icon.GetComponent<Image>();
            if (img != null)
            {
                Sprite sprite = Resources.Load<Sprite>(cardData.imgPath);
                if (sprite != null)
                    img.sprite = sprite;
            }
        }
    }
}
