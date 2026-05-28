[CardProperty(4,"扩容弹夹","","子弹上限+4\n血量+10\n散布范围min+10%")]

public class ExtendedMag : CardData
{
    public override string detailText => "子弹上限+4\n血量+10\n散布范围min+10%";
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.maxBulletCount += 4;
        pp.maxHp += 10;
        pp.minSpread *= 1.1f;
        GameState.Pm.UpdateProperty();
    }
}
