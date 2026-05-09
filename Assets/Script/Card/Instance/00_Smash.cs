using Framework.Gameplay;
using UnityEngine;

[CardProperty(0, "猛击", "", "暴击率+10%，攻击力+3")]
public class SmashCardData : CardData
{
    
    public override void OnChosen()
    {
        GameState.Pm.baseProperty.damage += 3;
        GameState.Pm.baseProperty.critRate += 10;
    }
}
