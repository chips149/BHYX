using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributesPanel : MonoBehaviour
{
    [Header("法宝属性底板（0=葫芦 1=印章 2=玉环）")]
    public Image[] weaponPanels;

    [Header("法宝属性显示")]
    public TMP_Text statDamage;       
    public TMP_Text statBulletMax;
    public TMP_Text statBulletReload;
    public TMP_Text statBulletScale;
    public TMP_Text statSpread;

    private int _weaponIndex = -1;

    private void Start()
    {
        RefreshPanel();
    }

    private int GetWeaponIndex()
    {
        var path = GameState.playerPath;
        if (string.IsNullOrEmpty(path)) return 0;
        if (path.Contains("HuLu")) return 0;
        if (path.Contains("WangBa")) return 1;
        if (path.Contains("YuHuan")) return 2;
        return 0;
    }

    public void RefreshPanel()
    {
        _weaponIndex = GetWeaponIndex();

        for (int i = 0; i < (weaponPanels?.Length ?? 0); i++)
            weaponPanels[i].enabled = (i == _weaponIndex);

        RefreshStats();
    }

    public void RefreshStats()
    {
        if (GameState.Pm?.finalProperty == null) return;

        var prop = GameState.Pm.finalProperty;

        if (statDamage != null)
            statDamage.text = FormatStat(prop.damage);
        if (statBulletMax != null)
            statBulletMax.text = $"{prop.maxBulletCount}";
        if (statBulletReload != null)
            statBulletReload.text = $"{FormatStat(prop.bulletReloadTime)}秒";
        if (statBulletScale != null)
            statBulletScale.text = FormatStat(prop.bulletScale);
        if (statSpread != null)
            statSpread.text = $"{FormatStat(prop.minSpread)}~{FormatStat(prop.maxSpread)}";
    }

    private static string FormatStat(float value)
    {
        return Mathf.Approximately(value, Mathf.Round(value))
            ? Mathf.RoundToInt(value).ToString()
            : value.ToString("0.##");
    }
}
