[CardProperty(2, "精妙子弹", "", "暴击伤害+20%，子弹恢复速度+5%，子弹大小-10%")]
public class DelicateBulletCardData : CardData
{
    public override void OnChosen()
    {
        GameState.Pm.baseProperty.critDamage += 20;
        GameState.Pm.baseProperty.bulletReloadTime *=0.95f;
        GameState.Pm.baseProperty.bulletScale -=0.1f;
        GameState.Pm.UpdateProperty();
    }
}
