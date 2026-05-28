[CardProperty(2, "精妙子弹", "UI/Card/UI_HUD_Bullet_Bar", "暴击伤害+20%\n子弹恢复速度+5%\n子弹大小-10%")]
public class DelicateBulletCardData : CardData
{
    public override string detailText => "暴击伤害+20%\n子弹恢复速度+5%\n子弹大小-10%";
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.critDamage += 20 * pp.critDamageCorrection;
        pp.bulletReloadTime *=0.95f;
        pp.bulletScale *= 0.90f;
        GameState.Pm.UpdateProperty();
    }
}
