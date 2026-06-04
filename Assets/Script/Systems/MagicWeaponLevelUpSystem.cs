using UnityEngine;

public static class MagicWeaponLevelUpSystem
{
    public const int MaxLevel = 99;

    private const int BaseCost = 100;
    private const int CostStep = 50;

    private static readonly string[] WeaponNames = { "HuLu", "WangBa", "YuHuan" };

    public static int GetLevel(string weaponName)
    {
        return PlayerPrefs.GetInt(GetLevelKey(weaponName), 0);
    }
    
    public static void SetLevel(string weaponName, int level)
    {
        PlayerPrefs.SetInt(GetLevelKey(weaponName), Mathf.Clamp(level, 0, MaxLevel));
        PlayerPrefs.Save();
    }
    
    public static void ResetAllLevels()
    {
        foreach (var name in WeaponNames)
        {
            PlayerPrefs.DeleteKey(GetLevelKey(name));
        }

        PlayerPrefs.Save();
    }

    public static bool CanLevelUp(string weaponName)
    {
        return GetLevel(weaponName) < MaxLevel;
    }

    public static bool UpLevel(string weaponName)
    {
        var currentLevel = GetLevel(weaponName);
        if (currentLevel >= MaxLevel) return false;

        var cost = GetUpgradeCost(currentLevel);
        if (!CoinSystem.SpendCoin(cost)) return false;

        SetLevel(weaponName, currentLevel + 1);
        return true;
    }
    
    public static int GetUpgradeCost(int currentLevel)
    {
        if (currentLevel >= MaxLevel) return 0;
        return BaseCost + currentLevel * CostStep; 
    }

    public static PlayerProperty GetBaseProperty(string weaponName)
    {
        var property = new PlayerProperty();

        switch (weaponName)
        {
            case "HuLu":
                property.damage = 2;
                property.attackInterval = 1f;
                property.maxBulletCount = 5;
                property.bulletReloadTime = 1f;
                property.bulletScale = 2f;
                property.maxHp = 35f;
                property.critRate = 5f;
                property.critDamage = 1.5f;
                property.maxSpread = 3f;
                property.minSpread = 5f;
                property.critRateCorrection = 1.5f;
                property.critDamageCorrection = 1.5f;
                break;
            case "WangBa":
                property.damage = 1;
                property.attackInterval = 1f;
                property.maxBulletCount = 5;
                property.bulletReloadTime = 0.8f;
                property.bulletScale = 2f;
                property.maxHp = 25f;
                property.critRate = 5f;
                property.critDamage = 1.5f;
                property.maxSpread = 3f;
                property.minSpread = 2f;
                property.damageCorrection = 1.25f;
                property.attackIntervalCorrection = 1.25f;
                break;
            case "YuHuan":
                property.damage = 1;
                property.attackInterval = 0.5f;
                property.maxBulletCount = 10;
                property.bulletReloadTime = 1f;
                property.bulletScale = 2f;
                property.maxHp = 30f;
                property.critRate = 5f;
                property.critDamage = 1.6f;
                property.maxSpread = 5f;
                property.minSpread = 3f;
                property.attackIntervalCorrection = 1.5f;
                break;
            default:
                property.damage = 1;
                property.attackInterval = 1f;
                property.maxBulletCount = 5;
                property.bulletReloadTime = 1f;
                property.bulletScale = 1f;
                property.maxHp = 30f;
                property.critRate = 5f;
                property.critDamage = 1.5f;
                property.maxSpread = 4.5f;
                property.minSpread = 2.5f;
                break;
        }

        return property;
    }

    public static PlayerProperty GetPropertyAtLevel(string weaponName, int level)
    {
        var property = GetBaseProperty(weaponName);
        ApplyUpLevel(property, Mathf.Clamp(level, 0, MaxLevel));
        return property;
    }

    public static void ApplyUpLevel(PlayerProperty property, int level)
    {
        property.damage += level * 2;
        property.maxBulletCount += level / 3;
        property.critRate += (level / 5) * 5;
        property.critDamage += (level / 2) * 0.05f;
        property.bulletReloadTime = Mathf.Max(0.1f, property.bulletReloadTime - level * 0.1f);
        property.maxHp += level * 3;
    }

    public static string GetDetailText(string weaponName, int level)
    {
        var property = GetPropertyAtLevel(weaponName, level);
        return $"攻击力：{Format(property.damage)}\n攻击间隔：{Format(property.attackInterval)}\n" +
               $"子弹上限：{property.maxBulletCount}\n子弹回复速度：{Format(property.bulletReloadTime)}秒\n子弹大小：{Format(property.bulletScale)}\n" +
               $"栅栏血量：{Format(property.maxHp)}\n暴击率：{Format(property.critRate)}%\n暴击伤害：+{Format((property.critDamage - 1f) * 100f)}%\n" +
               $"散布范围：{Format(property.minSpread)}~{Format(property.maxSpread)}";
    }

    public static string GetUpgradeCostText(string weaponName)
    {
        var currentLevel = GetLevel(weaponName);
        if (currentLevel >= MaxLevel) return "已满级";

        var cost = GetUpgradeCost(currentLevel);
        var coin = CoinSystem.GetCoin();
        var color = coin >= cost ? "green" : "red";
        return $": <color={color}>{cost}</color>";
    }

    public static string GetUpgradePreview(string weaponName, int currentLevel)
    {
        if (currentLevel >= MaxLevel) return "已满级，无法继续升级";

        var current = GetPropertyAtLevel(weaponName, currentLevel);
        var next = GetPropertyAtLevel(weaponName, currentLevel + 1);

        return $"下一级：{currentLevel + 1}\n" +
               GetDeltaLine("攻击力", current.damage, next.damage) +
               GetDeltaLine("子弹上限", current.maxBulletCount, next.maxBulletCount) +
               GetDeltaLine("子弹回复速度", current.bulletReloadTime, next.bulletReloadTime, "秒", true) +
               GetDeltaLine("栅栏血量", current.maxHp, next.maxHp) +
               GetDeltaLine("暴击率", current.critRate, next.critRate, "%") +
               GetDeltaLine("暴击伤害", (current.critDamage - 1f) * 100f, (next.critDamage - 1f) * 100f, "%");
    }

    public static string GetUpgradePreview(int currentLevel)
    {
        return GetUpgradePreview("HuLu", currentLevel);
    }

    private static string GetLevelKey(string weaponName)
    {
        return $"MagicWeapon_{weaponName}_Level";
    }

    private static string GetDeltaLine(string label, float current, float next, string suffix = "", bool lowerIsBetter = false)
    {
        var delta = next - current;
        if (Mathf.Approximately(delta, 0f)) return string.Empty;

        var sign = delta > 0f ? "+" : string.Empty;
        var color = lowerIsBetter && delta < 0f || !lowerIsBetter && delta > 0f ? "green" : "red";
        return $"{label}：{Format(current)}{suffix} <color={color}>{sign}{Format(delta)}{suffix}</color>\n";
    }

    private static string Format(float value)
    {
        return Mathf.Approximately(value, Mathf.Round(value)) ? Mathf.RoundToInt(value).ToString() : value.ToString("0.##");
    }
}
