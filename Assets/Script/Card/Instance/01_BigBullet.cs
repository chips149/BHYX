using Framework.Gameplay;

[CardProperty(1, "超大弹", "", "子弹大小+35%，攻击力+3，子弹回复速度-10%")]
public class BigBulletCardData : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.bulletScale *= 1.35f;
        pp.bulletReloadTime *= 0.90f;
        pp.attackInterval *= 0.85f;
        GameState.Pm.UpdateProperty();
    }
}
