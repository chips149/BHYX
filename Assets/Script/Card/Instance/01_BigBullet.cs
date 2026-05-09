using Framework.Gameplay;

[CardProperty(1, "超大弹", "", "子弹大小+35%，攻击力+3，子弹回复速度-10%")]
public class BigBulletCardData : CardData
{
    public override void OnChosen()
    {
        GameState.Pm.baseProperty.damage += 3;
        GameState.Pm.baseProperty.bulletScale += 0.35f;
        GameState.Pm.baseProperty.bulletReloadTime *= 1.1f;
        GameState.Pm.UpdateProperty();
    }
}
