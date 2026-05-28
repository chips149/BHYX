[CardProperty(6, "高速吟唱", "", "攻击间隔-15%\n子弹上限+2")]
public class FastChant : CardData
{
    public override string detailText => "攻击间隔-15%\n子弹上限+2";

    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.attackInterval *= 0.85f;
        pp.maxBulletCount += 2;
        GameState.Pm.UpdateProperty();
    }
}