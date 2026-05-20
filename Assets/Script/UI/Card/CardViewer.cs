
using UnityEngine;
using UnityEngine.UI;

public class CardViewer : MonoBehaviour
{
    private DrawCardPanel _drawCardPanel;
    private int _index;

    private CardData _cardData;

    public Image img;
    public Text description;

    private Button _btn;


    public void Initialize(DrawCardPanel drawCardPanel, int index, CardData data)
    {
        _drawCardPanel = drawCardPanel;
        _index = index;
        _cardData = data;
        
        Sprite sprite = Resources.Load<Sprite>(_cardData.imgPath);
        img.sprite = sprite;

        description.text = _cardData.description;

        _btn = GetComponent<Button>();
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        SaveData.AddCard(_cardData.id);
        _drawCardPanel.CloseDrawCardPanel();
        _cardData.OnChosen();
    }
    
}

    


