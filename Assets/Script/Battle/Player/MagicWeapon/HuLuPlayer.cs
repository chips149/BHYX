using System;

public class HuLuPlayer : PlayerBase
{
    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty = MagicWeaponLevelUpSystem.GetBaseProperty("HuLu");
        int level = MagicWeaponLevelUpSystem.GetLevel("HuLu");
        MagicWeaponLevelUpSystem.ApplyUpLevel(pm.baseProperty, level);
        
        pm.UpdateProperty();
        
        
        aimHandle.onAimEnd = ( aimPosition) =>
        {
            if (pm.bulletCount > 0 && pm.canAttack)
            {
                pm.bulletCount--;
                UpdateBulletUI();

                atkHandle.Attack(aimPosition);
            }
        };
    }
}