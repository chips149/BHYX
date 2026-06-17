using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDisplayCell : MonoBehaviour
{
    private MonsterPanel _manager;

    public int id;
    public Image img;
    public Button self;

    private void Awake()
    {
        img = GetComponent<Image>();
        self = GetComponent<Button>();
    }

    public void Initialize(MonsterPanel manager, MonsterInfo info)
    {
        _manager = manager;
        img.sprite = Resources.Load<Sprite>(info.imgPath);
    }

    public void SetSelected(bool selected)
    {
        transform.DOScale(selected ? Vector3.one * 1.15f : Vector3.one, 0.2f);
    }

    public void OnClick()
    {
        _manager.Display(id);
    }
}

