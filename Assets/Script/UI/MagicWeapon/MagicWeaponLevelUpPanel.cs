using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicWeaponLevelUpPanel : MonoBehaviour
{
    [Serializable]
    public class WeaponModule
    {
        [Header("Left")]
        public Image icon;
        public Text nameText;
        public Text descriptionText;
        public Text mechanismText;
        public Text valueText;

        [Header("Right")]
        public Text levelText;
        public Text previewText;
        public Button upgradeButton;
    }

    [Serializable]
    public class WeaponBaseStats
    {
        public float damage;
        public float attackInterval;
        public int maxBulletCount;
        public float bulletReloadTime;
        public float bulletScale;
        public float maxHp;
        public int critRate; 
        public int critDamagePercent;
        public string spreadRange;
    }

    [Header("Data")]
    public ConfigAsset displayAsset;
    public string[] weaponKeys = { "HuLu", "WangBa", "YuHuan" };
    public string[] weaponMechanisms =
    {
        "高额提升加成：暴击率修正、暴击伤害修正",
        "每攻击 3 次额外释放一次封印攻击",
        "命中后使用累计卡牌伤害，空枪后攻击力 +1"
    };

    [Header("各法宝基础属性")]
    public WeaponBaseStats[] baseStats = new WeaponBaseStats[3];

    public List<WeaponModule> modules = new();

    private readonly List<UnityEngine.Events.UnityAction> upgradeActions = new();

    private void Awake()
    {
        BindUpgradeButtons();
    }

    private void OnEnable()
    {
        RefreshAll();
    }

    private void OnDestroy()
    {
        UnbindUpgradeButtons();
    }

    private void BindUpgradeButtons()
    {
        UnbindUpgradeButtons();

        for (var i = 0; i < modules.Count; i++)
        {
            if (modules[i].upgradeButton == null)
            {
                upgradeActions.Add(null);
                continue;
            }

            var index = i;
            UnityEngine.Events.UnityAction action = () => Upgrade(index);
            upgradeActions.Add(action);
            modules[i].upgradeButton.onClick.AddListener(action);
        }
    }

    private void UnbindUpgradeButtons()
    {
        for (var i = 0; i < upgradeActions.Count && i < modules.Count; i++)
        {
            if (modules[i].upgradeButton != null && upgradeActions[i] != null)
            {
                modules[i].upgradeButton.onClick.RemoveListener(upgradeActions[i]);
            }
        }

        upgradeActions.Clear();
    }

    public void RefreshAll()
    {
        for (var i = 0; i < modules.Count; i++)
        {
            RefreshModule(i);
        }
    }

    private void RefreshModule(int index)
    {
        if (!IsValidIndex(index)) return;

        var module = modules[index];
        var info = displayAsset.list[index];
        var weaponKey = GetWeaponKey(index, info.name);
        var level = MagicWeaponLevelUpSystem.GetLevel(weaponKey);

        if (module.icon != null)
        {
            module.icon.sprite = Resources.Load<Sprite>(string.IsNullOrEmpty(info.iconPath) ? info.imgPath : info.iconPath);
        }

        SetText(module.nameText, $"{info.name}  Lv.{level}");
        SetText(module.descriptionText, info.description);
        SetText(module.mechanismText, GetWeaponMechanism(index));
        SetText(module.valueText, BuildValueText(index, level));
        SetText(module.levelText, $"当前等级：{level}\n升级后：{level + 1}");
        SetText(module.previewText, MagicWeaponLevelUpSystem.GetUpgradePreview(level));
    }

    private void Upgrade(int index)
    {
        if (!IsValidIndex(index)) return;

        var info = displayAsset.list[index];
        var weaponKey = GetWeaponKey(index, info.name);
        if (MagicWeaponLevelUpSystem.UpLevel(weaponKey))
        {
            RefreshModule(index);
        }
    }

    private bool IsValidIndex(int index)
    {
        return displayAsset != null
               && displayAsset.list != null
               && index >= 0
               && index < displayAsset.list.Count
               && index < modules.Count;
    }

    private string GetWeaponKey(int index, string fallbackName)
    {
        if (weaponKeys != null && index < weaponKeys.Length && !string.IsNullOrEmpty(weaponKeys[index]))
        {
            return weaponKeys[index];
        }

        return fallbackName;
    }

    private string GetWeaponMechanism(int index)
    {
        if (weaponMechanisms != null && index < weaponMechanisms.Length && !string.IsNullOrEmpty(weaponMechanisms[index]))
        {
            return weaponMechanisms[index];
        }

        return "特殊机制：未配置";
    }

    private string BuildValueText(int index, int level)
    {
        if (baseStats == null || index < 0 || index >= baseStats.Length || baseStats[index] == null)
        {
            return string.Empty;
        }

        var s = baseStats[index];
        var damage = s.damage + level * 2;
        var maxBullet = s.maxBulletCount + level / 3;
        var reloadTime = Mathf.Max(0.1f, s.bulletReloadTime - level * 0.1f);
        var hp = s.maxHp + level * 3;
        var critRate = s.critRate + (level / 5) * 5;
        var critDamagePercent = s.critDamagePercent + (level / 2) * 5;

        return $"攻击力：{damage}\n" +
               $"攻击间隔：{s.attackInterval}\n" +
               $"子弹上限：{maxBullet}\n" +
               $"子弹回复速度：{reloadTime:F1}\n" +
               $"子弹大小：{s.bulletScale}\n" +
               $"栅栏血量：{hp}\n" +
               $"暴击率：{critRate}%\n" +
               $"暴击伤害：+{critDamagePercent}%\n" +
               $"散布范围：{s.spreadRange}";
    }

    private static void SetText(Text target, string content)
    {
        if (target != null)
        {
            target.text = content;
        }
    }
}
