[CardProperty(3,"精准","UI/Card/UI_HUD_Precise_Bar","最大散步范围-25%\n血量-5")]
public class PreciseCardData:CardData
{
    public override string detailText => "最大散步范围-25%\n血量-5";
    public override void OnChosen()
    {
        var pp=GameState.Pm.baseProperty;
        pp.maxSpread *= 0.75f;
        pp.maxHp -= 5;
        GameState.Pm.UpdateProperty();
    }
}