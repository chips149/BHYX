using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCell : MonoBehaviour
{
    private MagicWeaponPanel _manager;

    public int id;
    public Image img;

    public Sprite displayIcon;
    public string mwName;
    public string description;
    public string boostAdjust;

    public Button self;

    private void Awake()
    {
        self = GetComponent<Button>();
    }

    public void Initialize(MagicWeaponPanel manager, MagicWeaponInfo info)
    {
        _manager = manager;
        mwName = info.name;
        img.sprite = Resources.Load<Sprite>(info.imgPath);
        description = info.description;
        boostAdjust = info.boostAdjust;
    }

    public void SetSelected(bool selected)
    {
        transform.DOScale(selected ? Vector3.one * 1.15f : Vector3.one, 0.2f);
    }

    public void OnClick()
    {
        _manager.Display(this);
    }
}
