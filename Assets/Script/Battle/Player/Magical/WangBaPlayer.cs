using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WangBaPlayer : PlayerBase
{
    private int attackCount;
    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty.damage = 1;
        pm.baseProperty.attackInterval = 1;
        pm.baseProperty.maxBulletCount = 7;
        pm.baseProperty.bulletReloadTime = 0.8f;
        pm.baseProperty.bulletScale = 2;
        pm.baseProperty.maxHp = 30;
        pm.baseProperty.critRate = 5;
        pm.baseProperty.critDamage = 1.5f;
        pm.baseProperty.maxSpread = 3;
        pm.baseProperty.minSpread = 2;

        pm.baseProperty.damageCorrection = 1.25f;
        pm.baseProperty.attackIntervalCorrection = 1.25f;
        
        pm.UpdateProperty();

        var originalOnAimEnd = aimHandle.onAimEnd;
        
        aimHandle.onAimEnd = (Vector3 aimPosition) =>
        {
            originalOnAimEnd?.Invoke(aimPosition);
            
            attackCount++;

            if (attackCount >= 3)
            {
                attackCount = 0;

                Vector3 sealSpawnPos = playerPoint.position + Vector3.up * 10f;

                DefaultSealBullet.Shoot(sealSpawnPos, pm, pm.GetFinalDamage() * 2); 
            }
        };
    }
}
