[CardProperty(7, "快速装填", "", "子弹回复速度+15%,暴击伤害+10%,攻击间隔+5%")]
public class QuickReload : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.bulletReloadTime *= 0.85f;
        pp.critDamage *= 1.1f;
        pp.attackInterval *= 1.05f;
        GameState.Pm.UpdateProperty();
    }
}
