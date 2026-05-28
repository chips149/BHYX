using Framework.Gameplay;

[CardProperty(1, "超大弹", "UI/Card/UI_HUD_HugeBullet_Bar", "子弹大小+35%\n攻击力+3\n子弹回复速度-10%")]
public class BigBulletCardData : CardData
{
    public override string detailText => "子弹大小+35%\n攻击力+3\n子弹回复速度-10%";
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;
        pp.bulletScale *= 1.35f;
        pp.bulletReloadTime *= 0.90f;
        pp.attackInterval *= 0.85f;
        GameState.Pm.UpdateProperty();
    }
}
