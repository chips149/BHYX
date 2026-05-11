[CardProperty(3,"精准","","最大散步范围-25%，血量-5")]
public class PreciseCardData:CardData
{
    public override void OnChosen()
    {
        var pp=GameState.Pm.baseProperty;
        pp.maxSpread *= 0.75f;
        pp.maxHp -= 5;
        GameState.Pm.UpdateProperty();
    }
}