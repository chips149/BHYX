using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangBaPlayer : PlayerBase
{
    private int attackCount;

    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty = MagicWeaponLevelUpSystem.GetBaseProperty("WangBa");
        int level = MagicWeaponLevelUpSystem.GetLevel("WangBa");
        MagicWeaponLevelUpSystem.ApplyUpLevel(pm.baseProperty, level);

        pm.UpdateProperty();

        aimHandle.onAimEnd = (aimPosition) =>
        {
            if (pm.bulletCount > 0 && pm.canAttack)
            {
                pm.bulletCount--;
                attackCount++;
                UpdateBulletUI();

                if (attackCount >= 3)
                {
                    attackCount = 0;

                    DefaultBullet.Shoot(playerPoint.position, aimPosition, pm, onLanded: landedPos =>
                    {
                        DefaultSealBullet.Shoot(landedPos, pm, pm.baseProperty.damage);
                    });
                }
                else
                {
                    atkHandle.Attack(aimPosition);
                }
            }
        };
    }
}