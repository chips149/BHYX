using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWeaponLevelUpSystem
{
    public static int GetLevel(string weaponName)
    {
        return PlayerPrefs.GetInt($"MagicWeapon_{weaponName}_Level", 0);
    }
    
    public static void SetLevel(string weaponName,int level)
    {
        PlayerPrefs.SetInt($"MagicWeapon_{weaponName}_Level",level);
        PlayerPrefs.Save();
    }

    public static bool UpLevel(string weaponName)
    {
        var currentLevel = GetLevel(weaponName);
        SetLevel(weaponName,currentLevel+1);
        Debug.Log($"法宝升级到{currentLevel+1}级");
        return true;
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
    
    public static string GetUpgradePreview(PlayerProperty baseProperty, int currentLevel)
    {
        int nextLevel = currentLevel + 1;
        return $"下一级 ({nextLevel} 级) 将提升：\n" +
               $"攻击力 +2\n" +
               $"{(nextLevel % 3 == 0 ? "子弹上限 +1\n" : "")}" +
               $"子弹回复速度 -0.1秒\n" +
               $"血量 +3\n" +
               $"{(nextLevel % 5 == 0 ? "暴击率 +5%\n" : "")}" +
               $"{(nextLevel % 2 == 0 ? "暴击伤害 +5%\n" : "")}";
    }
}
