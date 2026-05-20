using Framework.Gameplay;
using UnityEngine;

[CardProperty(0, "猛击", "", "暴击率+10%，子弹大小+15%")]
public class SmashCardData : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        
        pp.critRate += 10 * pp.critRateCorrection;
        pp.bulletScale *= 1.15f;
        GameState.Pm.UpdateProperty();
    }
}
