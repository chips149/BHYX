[CardProperty(5, "强攻", "", "暴击率+10%\n攻击间隔+5%\n子弹大小-15%")]

public class StrongAttack : CardData
{
    public override string detailText => "暴击率+10%\n攻击间隔+5%\n子弹大小-15%"; 

    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.critRate += 10 * pp.critRateCorrection;
        pp.attackInterval *= 1.05f;
        pp.bulletScale *= 0.85f;
        GameState.Pm.UpdateProperty();
    }
} 