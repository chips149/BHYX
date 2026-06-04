using UnityEngine;
using UnityEngine.UI;

public class DrawCardPanel : MonoBehaviour
{
    private CardViewer[] _viewers;
    [SerializeField] private Button refreshButton;

    private int _selectedIndex = -1;
    private CardData _selectedCardData;

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
        CloseDrawCardPanel();
    }

    public void OpenDrawCardPanel()
    {
        gameObject.SetActive(true);
        RandomCard();
    }

    public void CloseDrawCardPanel()
    {
        gameObject.SetActive(false);
        GameState.currentLevel++;
        SaveManager.ToSave();
        SpawnMonsterHandler.Instance.StartSpawn();
    }

    public void OnRefreshClicked()
    {
        RandomCard();
    }
}
