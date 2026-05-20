[CardProperty(4,"扩容弹夹","","子弹上限+4,血量+10,散布范围min+10%")]

public class ExtendedMag : CardData
{
    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.maxBulletCount += 4;
        pp.maxHp += 10;
        pp.minSpread *= 1.1f;
        GameState.Pm.UpdateProperty();
    }
}
