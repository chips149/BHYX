[CardProperty(5, "强攻", "UI/Card/Icon/StrongAttack", "暴击率+10%\n子弹大小-15%")]

public class StrongAttackCardData : CardData
{
    public override string detailText => "暴击率+10%\n子弹大小-15%"; 

    public override void OnChosen()
    {
        var pp = GameState.Pm.baseProperty;

        pp.critRate += 10 * pp.critRateCorrection;
        pp.bulletScale *= 0.85f;
        GameState.Pm.UpdateProperty();
    }
} 