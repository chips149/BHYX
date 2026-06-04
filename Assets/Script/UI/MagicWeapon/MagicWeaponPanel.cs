using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class MagicWeaponPanel : MonoBehaviour
{
    public MagicWeaponConfig display;

    [Header("Reference")]
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text boostAdjust;
    public TMP_Text detail;
    public TMP_Text bonusPreview;
    public TMP_Text upgradeCost;
    public Button upgradeButton;
    public GameObject noHaveCoinImage;

    public Transform parent;

    public int selectedID = -1;

    public string[] playerPrefab;
    public string[] weaponLevelKeys = { "HuLu", "WangBa", "YuHuan" };

    public readonly List<DisplayCell> Cells = new();
    
    public UnityEvent onAfterInitialize;
    public UnityEvent onUpgradeFailed;
    public UnityEvent onUpgradeSucceeded;
    
    private void Start()
    {
        Initialize();
        
        onAfterInitialize?.Invoke();
    }

    private void Initialize()
    {
        var prefab = Resources.Load<DisplayCell>(display.prefabPath);

        for (var i = 0; i < display.list.Count; i++)
        {
            var info = display.list[i];
            var behavior = Instantiate(prefab, parent.transform);
            behavior.id = i;
            behavior.Initialize(this, info);
            Cells.Add(behavior);
        }

        upgradeButton.onClick.RemoveListener(UpgradeSelectedWeapon);
        upgradeButton.onClick.AddListener(UpgradeSelectedWeapon);

        var noCoinAnim = GetComponent<NoCoinAnimation>();
        if (noCoinAnim != null && noHaveCoinImage != null)
            onUpgradeFailed.AddListener(() => noCoinAnim.Play(noHaveCoinImage));

        Display(Cells.First());
    }

    public void Display(DisplayCell cell)
    {
        selectedID = cell.id;
        nameText.text = cell.mwName;
        description.text = cell.description;
        boostAdjust.text = cell.boostAdjust;
        RefreshUpgradeInfo();
    }

    public void UpgradeSelectedWeapon()
    {
        if (!TryGetSelectedWeaponKey(out var weaponKey)) return;

        if (!MagicWeaponLevelUpSystem.UpLevel(weaponKey))
        {
            onUpgradeFailed?.Invoke();
            RefreshUpgradeInfo();
            return;
        }

        onUpgradeSucceeded?.Invoke();
        RefreshUpgradeInfo();
        SaveManager.ToSave();
    }

    public void ConfirmSelection()
    {
        GameState.playerPath = playerPrefab[selectedID];
        SaveManager.ToSave();
    }

    private void RefreshUpgradeInfo()
    {
        if (!TryGetSelectedWeaponKey(out var weaponKey)) return;

        var currentLevel = MagicWeaponLevelUpSystem.GetLevel(weaponKey);

        detail.text = MagicWeaponLevelUpSystem.GetDetailText(weaponKey, currentLevel);

        bonusPreview.text = MagicWeaponLevelUpSystem.GetUpgradePreview(weaponKey, currentLevel);

        upgradeCost.text = MagicWeaponLevelUpSystem.GetUpgradeCostText(weaponKey);

        var cost = MagicWeaponLevelUpSystem.GetUpgradeCost(currentLevel);
        upgradeButton.interactable = MagicWeaponLevelUpSystem.CanLevelUp(weaponKey);
    }
    

    private bool TryGetSelectedWeaponKey(out string weaponKey)
    {
        weaponKey = string.Empty;

        if (selectedID < 0 || selectedID >= weaponLevelKeys.Length)
        {
            return false;
        }

        weaponKey = weaponLevelKeys[selectedID];
        return !string.IsNullOrEmpty(weaponKey);
    }
}
