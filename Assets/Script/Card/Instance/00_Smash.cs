using Framework.Gameplay;
using UnityEngine;

[CardProperty(0, "猛击", "UI/Card/Icon/Smash", "暴击率+10%\n暴击伤害+15%")]
public class SmashCardData : CardData
{
    public override string detailText => "暴击率+10%\n暴击伤害+15%";
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        
        pp.critRate += 10 * pp.critRateCorrection;
        pp.critDamage +=15* pp.critDamage;
        GameState.Pm.UpdateProperty();
    }
}
