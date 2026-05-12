using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuHuanPlayer : PlayerBase
{
    public float finalCardDamage;
    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty.damage = 1;
        pm.baseProperty.attackInterval = 0.8f;
        pm.baseProperty.maxBulletCount = 7;
        pm.baseProperty.bulletReloadTime = 1.2f;
        pm.baseProperty.bulletScale = 2;
        pm.baseProperty.maxHp = 30;
        pm.baseProperty.critRate = 5;
        pm.baseProperty.critDamage = 1.5f;
        pm.baseProperty.maxSpread = 7;
        pm.baseProperty.minSpread = 5;

        pm.baseProperty.attackIntervalCorrection = 1.5f;

        finalCardDamage = pm.baseProperty.damage;
        pm.OnBulletHit += OnHit;
        pm.OnBulletMiss += OnMiss;
        
        pm.UpdateProperty();
    }
    public override void OnCardDamage(float amount)
    {
        finalCardDamage += amount;
    }
    
    private void OnHit()
    {
        pm.baseProperty.damage = finalCardDamage;
        pm.UpdateProperty();
    }
    
    private void OnMiss()
    {
        pm.baseProperty.damage += 1;
        pm.UpdateProperty();
    }
}
