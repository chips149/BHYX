using UnityEngine;

public class DrawCardPanel : MonoBehaviour
{
    private CardViewer[] _viewers;

    void RandomCard()
    {
        _viewers??=transform.GetComponentsInChildren<CardViewer>();
        var data = CardHandler.RandomCardData();
        for (var i = 0; i < _viewers.Length; i++)
        {
            _viewers[i].Initialize(this,i, data[i]);
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
    }
}
