using Framework.Gameplay;
using UnityEngine;

[CardProperty(0, "猛击", "", "暴击率+10%，攻击力+3")]
public class SmashCardData : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.damage += 3 * pp.damageCorrection;
        pp.critRate += 10 * pp.critRateCorrection;
        GameState.Pm.UpdateProperty();
    }
}
