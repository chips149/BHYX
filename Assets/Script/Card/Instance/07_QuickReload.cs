[CardProperty(7, "快速装填", "UI/Card/Icon/QuickReload", "子弹回复速度+15%\n暴击伤害+10%")]
public class QuickReloadCardData : CardData
{
    public override string detailText => "子弹回复速度+15%\n暴击伤害+10%";

    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.bulletReloadTime *= 0.85f;
        pp.critDamage *= 1.1f;
        GameState.Pm.UpdateProperty();
    }
}
