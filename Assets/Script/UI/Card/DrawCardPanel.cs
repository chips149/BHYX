using UnityEngine;
using UnityEngine.UI;

public class DrawCardPanel : MonoBehaviour
{
    private CardViewer[] _viewers;
    [SerializeField] private Button refreshButton;

    void RandomCard()
    {
        _viewers ??= transform.GetComponentsInChildren<CardViewer>();
        var data = CardHandler.RandomCardData();
        for (var i = 0; i < _viewers.Length && i < data.Length; i++)
        {
            _viewers[i].Initialize(this, i, data[i]);
        }
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
