[CardProperty(3,"精准","","最大散步范围-25%，血量-5")]
public class PreciseCardData:CardData
{
    public override void OnChosen()
    {
        GameState.Pm.baseProperty.maxSpread *= 0.75f;
        GameState.Pm.baseProperty.maxHp -= 5;
        GameState.Pm.UpdateProperty();
    }
}