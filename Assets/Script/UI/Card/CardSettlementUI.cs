using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardSettlementUI : MonoBehaviour
{
    private const int ColumnCount = 3;

    [Header("卡牌预制体")]
    [SerializeField] private GameObject itemPrefab;

    [Header("三列父节点")]
    [SerializeField] private RectTransform contentParent;

    [Header("布局参数")]
    [SerializeField] private float columnSpacing = 24f;
    [SerializeField] private float expandedHeight = 720f;
    [SerializeField] private float expandedItemSpacing = 18f;
    [SerializeField] private float collapsedEdgeHeight = 36f;

    [Header("DOTween 动画参数")]
    [SerializeField] private float animDuration = 0.35f;
    [SerializeField] private float staggerDelay = 0.04f;
    [SerializeField] private Ease expandEase = Ease.OutCubic;
    [SerializeField] private Ease collapseEase = Ease.OutBack;

    private readonly List<CardColumn> columns = new();
    private int expandedColumnIndex = -1;

    public void Show()
    {
        var cardIds = SaveData.Instance.chosenCardIds;
        if (cardIds == null || cardIds.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (itemPrefab == null || contentParent == null || CardHandler.Data == null || CardHandler.Data.Length == 0) { gameObject.SetActive(false); return; }

        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        Canvas.ForceUpdateCanvases();

        ClearCards();
        DisableParentLayoutComponents();
        BuildColumns();

        var cardMap = CardHandler.Data.ToDictionary(c => c.id, c => c);
        for (int i = 0; i < cardIds.Count; i++)
        {
            if (!cardMap.TryGetValue(cardIds[i], out var cardData)) continue;

            var column = columns[i % ColumnCount];
            var itemObj = Instantiate(itemPrefab, column.content);
            var rt = itemObj.GetComponent<RectTransform>();
            FillCard(itemObj, cardData);
            EnsureVisibleCardBackground(itemObj);

            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y);
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.zero;
            rt.gameObject.SetActive(true);

            var canvasGroup = itemObj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = itemObj.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            column.items.Add(rt);
            column.groups.Add(canvasGroup);
        }

        for (int i = 0; i < columns.Count; i++)
        {
            LayoutColumn(i, false, true);
            PlayEnterAnimation(columns[i], i);
        }
    }

    private void BuildColumns()
    {
        expandedColumnIndex = -1;
        columns.Clear();

        float parentWidth = contentParent.rect.width > 0f ? contentParent.rect.width : 900f;
        float columnWidth = (parentWidth - columnSpacing * (ColumnCount - 1)) / ColumnCount;
        float startX = -parentWidth * 0.5f + columnWidth * 0.5f;

        for (int i = 0; i < ColumnCount; i++)
        {
            var root = CreateRect($"CardColumn_{i + 1}", contentParent);
            root.anchorMin = new Vector2(0.5f, 1f);
            root.anchorMax = new Vector2(0.5f, 1f);
            root.pivot = new Vector2(0.5f, 1f);
            root.sizeDelta = new Vector2(columnWidth, expandedHeight);
            root.anchoredPosition = new Vector2(startX + i * (columnWidth + columnSpacing), 0f);

            var viewport = CreateRect("Viewport", root);
            viewport.anchorMin = new Vector2(0f, 1f);
            viewport.anchorMax = new Vector2(1f, 1f);
            viewport.pivot = new Vector2(0.5f, 1f);
            viewport.sizeDelta = new Vector2(0f, expandedHeight);
            viewport.anchoredPosition = Vector2.zero;

            var viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.color = new Color(1f, 1f, 1f, 0.001f);
            viewportImage.raycastTarget = true;
            viewport.gameObject.AddComponent<RectMask2D>();

            var content = CreateRect("Content", viewport);
            content.anchorMin = new Vector2(0.5f, 1f);
            content.anchorMax = new Vector2(0.5f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.sizeDelta = new Vector2(columnWidth, expandedHeight);
            content.anchoredPosition = Vector2.zero;

            var scrollRect = viewport.gameObject.AddComponent<ScrollRect>();
            scrollRect.viewport = viewport;
            scrollRect.content = content;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.inertia = true;
            scrollRect.enabled = false;

            int columnIndex = i;
            var button = viewport.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(() => ToggleColumn(columnIndex));

            columns.Add(new CardColumn
            {
                root = root,
                viewport = viewport,
                content = content,
                scrollRect = scrollRect
            });
        }
    }

    private void ToggleColumn(int columnIndex)
    {
        bool shouldExpand = expandedColumnIndex != columnIndex;

        if (expandedColumnIndex >= 0 && expandedColumnIndex < columns.Count)
            LayoutColumn(expandedColumnIndex, false, false);

        expandedColumnIndex = shouldExpand ? columnIndex : -1;
        LayoutColumn(columnIndex, shouldExpand, false);
    }

    private void LayoutColumn(int columnIndex, bool expanded, bool instant)
    {
        var column = columns[columnIndex];
        float cardHeight = GetCardHeight(column);
        float collapsedHeight = GetCollapsedHeight(column, cardHeight);
        float targetViewportHeight = expanded ? expandedHeight : collapsedHeight;
        float itemStep = expanded ? cardHeight + expandedItemSpacing : collapsedEdgeHeight;
        float contentHeight = Mathf.Max(targetViewportHeight, cardHeight + Mathf.Max(0, column.items.Count - 1) * itemStep);

        column.scrollRect.enabled = expanded;
        column.scrollRect.verticalNormalizedPosition = 1f;
        column.root.SetAsLastSibling();

        if (instant)
        {
            column.viewport.sizeDelta = new Vector2(column.viewport.sizeDelta.x, targetViewportHeight);
            column.content.sizeDelta = new Vector2(column.content.sizeDelta.x, contentHeight);
        }
        else
        {
            column.viewport.DOKill();
            column.content.DOKill();
            column.viewport.DOSizeDelta(new Vector2(column.viewport.sizeDelta.x, targetViewportHeight), animDuration).SetEase(expanded ? expandEase : collapseEase).SetLink(column.viewport.gameObject);
            column.content.DOSizeDelta(new Vector2(column.content.sizeDelta.x, contentHeight), animDuration).SetEase(expandEase).SetLink(column.content.gameObject);
        }

        for (int i = 0; i < column.items.Count; i++)
        {
            var item = column.items[i];
            var group = column.groups[i];
            var targetPos = new Vector2(0f, -i * itemStep);
            item.SetSiblingIndex(i);
            item.DOKill();

            if (instant)
            {
                item.anchoredPosition = targetPos;
                group.alpha = 1f;
            }
            else
            {
                item.DOAnchorPos(targetPos, animDuration).SetEase(expanded ? expandEase : collapseEase).SetLink(item.gameObject);
                group.DOFade(1f, animDuration * 0.8f).SetLink(item.gameObject);
            }
        }
    }

    private void PlayEnterAnimation(CardColumn column, int columnIndex)
    {
        for (int i = 0; i < column.items.Count; i++)
        {
            var rt = column.items[i];
            var group = column.groups[i];
            var seq = DOTween.Sequence();
            seq.AppendInterval((columnIndex + i) * staggerDelay);
            seq.AppendCallback(() =>
            {
                rt.gameObject.SetActive(true);
                group.alpha = 0f;
            });
            seq.Join(group.DOFade(1f, animDuration * 0.8f));
            seq.Join(rt.DOScale(1f, animDuration).SetEase(collapseEase));
            seq.SetLink(rt.gameObject);
        }
    }

    private float GetCollapsedHeight(CardColumn column, float cardHeight)
    {
        if (column.items.Count == 0) return 0f;
        return cardHeight + Mathf.Max(0, column.items.Count - 1) * collapsedEdgeHeight;
    }

    private float GetCardHeight(CardColumn column)
    {
        for (int i = 0; i < column.items.Count; i++)
        {
            if (column.items[i] != null && column.items[i].rect.height > 0f)
                return column.items[i].rect.height;
        }

        var prefabRect = itemPrefab.GetComponent<RectTransform>();
        return prefabRect != null && prefabRect.rect.height > 0f ? prefabRect.rect.height : 320f;
    }

    private RectTransform CreateRect(string objectName, Transform parent)
    {
        var go = new GameObject(objectName, typeof(RectTransform));
        go.layer = parent.gameObject.layer;
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);
        return rt;
    }

    private void FillCard(GameObject itemObj, CardData cardData)
    {
        var icon = itemObj.transform.Find("Icon")?.GetComponent<Image>();
        if (icon != null)
        {
            if (!string.IsNullOrEmpty(cardData.imgPath))
            {
                var sprite = Resources.Load<Sprite>(cardData.imgPath);
                if (sprite != null)
                    icon.sprite = sprite;
            }

            icon.enabled = true;
        }

        var nameText = itemObj.transform.Find("NameText")?.GetComponent<Text>();
        if (nameText != null)
        {
            nameText.text = cardData.name;
            nameText.color = Color.white;
        }

        var descText = itemObj.transform.Find("DescriptionText")?.GetComponent<Text>();
        if (descText != null)
        {
            descText.text = cardData.description;
            descText.color = Color.white;
        }
    }

    private void EnsureVisibleCardBackground(GameObject itemObj)
    {
        var background = itemObj.GetComponent<Image>();
        if (background == null)
            background = itemObj.AddComponent<Image>();

        if (background.sprite == null)
        {
            background.sprite = Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0f, 0f, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height),
                new Vector2(0.5f, 0.5f));
        }

        background.type = Image.Type.Sliced;
        background.color = new Color(0.92f, 0.85f, 0.5f, 1f);
    }

    private void DisableParentLayoutComponents()
    {
        foreach (var layout in contentParent.GetComponents<LayoutGroup>())
            layout.enabled = false;

        var fitter = contentParent.GetComponent<ContentSizeFitter>();
        if (fitter != null) fitter.enabled = false;
    }

    private void ClearCards()
    {
        if (contentParent == null) return;

        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            var child = contentParent.GetChild(i).gameObject;
            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        columns.Clear();
        expandedColumnIndex = -1;
    }

    private class CardColumn
    {
        public RectTransform root;
        public RectTransform viewport;
        public RectTransform content;
        public ScrollRect scrollRect;
        public readonly List<RectTransform> items = new();
        public readonly List<CanvasGroup> groups = new();
    }
}
