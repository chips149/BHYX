using UnityEngine;

[CardProperty(14, "疯狂钻石", "UI/Card/Icon/CrazyDiamond", "回复50%血量\n最小散布范围+20%")]

public class CrazyDiamondCardData : CardData
{
    public override string detailText => "回复50%血量\n最小散布范围+20%"; 
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        var ph = GameState.Pm.playerHealth;
        
        pp.minSpread *= 1.2f;

        var heal = Mathf.RoundToInt(ph.currentHp * 0.5f);
        ph.currentHp = Mathf.Min(ph.currentHp + heal, ph.maxHp);
        ph.UpdateHealthDisplay();
    }
}

