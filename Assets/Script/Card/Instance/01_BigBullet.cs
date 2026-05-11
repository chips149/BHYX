using Framework.Gameplay;

[CardProperty(1, "超大弹", "", "子弹大小+35%，攻击力+3，子弹回复速度-10%")]
public class BigBulletCardData : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.damage += 3 * pp.damageCorrection;
        pp.bulletScale += 0.35f;
        pp.bulletReloadTime *= 1.1f;
        GameState.Pm.UpdateProperty();
    }
}
