[CardProperty(2, "精妙子弹", "", "暴击伤害+20%，子弹恢复速度+5%，子弹大小-10%")]
public class DelicateBulletCardData : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.critDamage += 20 * pp.critDamageCorrection;
        pp.bulletReloadTime *=0.95f;
        pp.bulletScale -=0.1f;
        GameState.Pm.UpdateProperty();
    }
}
