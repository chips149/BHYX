using System.Collections;
using System.Collections.Generic;
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
    public string detail;

    public Button self;

    private void Awake()
    {
        self = GetComponent<Button>();
    }

    public void Initialize(MagicWeaponPanel manager, ConfigInfo info)
    {
        _manager = manager;
        //
        mwName = info.name;
        img.sprite = Resources.Load<Sprite>(info.imgPath);
        displayIcon = Resources.Load<Sprite>(info.iconPath);
        description = info.description;
        detail = info.detail;
    }

    public void OnClick()
    {
        _manager.Display(this);
    }
}
