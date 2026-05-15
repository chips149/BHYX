using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WangBaPlayer : PlayerBase
{
    private int attackCount;
    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty.damage = 1;
        pm.baseProperty.attackInterval = 1;
        pm.baseProperty.maxBulletCount = 5;
        pm.baseProperty.bulletReloadTime = 0.8f;
        pm.baseProperty.bulletScale = 2;
        pm.baseProperty.maxHp = 25;
        pm.baseProperty.critRate = 5;
        pm.baseProperty.critDamage = 1.5f;
        pm.baseProperty.maxSpread = 3;
        pm.baseProperty.minSpread = 2;

        pm.baseProperty.damageCorrection = 1.25f;
        pm.baseProperty.attackIntervalCorrection = 1.25f;
       
        int level = MagicWeaponLevelUpSystem.GetLevel("WangBa");
        MagicWeaponLevelUpSystem.ApplyUpLevel(pm.baseProperty, level);
        
        pm.UpdateProperty();
        
        aimHandle.onAimEnd = ( aimPosition) =>
        {
            if (pm.bulletCount > 0 && pm.canAttack)
            {
                pm.bulletCount--;
                attackCount++;
                UpdateBulletUI();

                if (attackCount >= 3)
                {
                    attackCount = 0;
                    DefaultBullet.Shoot(playerPoint.position, aimPosition, pm, async landedPosition =>
                    {
                        await UniTask.Delay(500);
                        DefaultSealBullet.Shoot(landedPosition, pm, pm.baseProperty.damage);
                    });
                }
                else
                {
                    atkHandle.Attack(aimPosition);
                }

                pm.canAttack = false;
                pm.attackCooldownTimer = 0;
            }
        };
    }
}
