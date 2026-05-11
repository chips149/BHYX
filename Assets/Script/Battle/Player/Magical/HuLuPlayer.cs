using System;

public class HuLuPlayer : PlayerBase
{
    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty.damage = 2;
        pm.baseProperty.attackInterval = 1;
        pm.baseProperty.maxBulletCount = 5;
        pm.baseProperty.bulletReloadTime = 1;
        pm.baseProperty.bulletScale = 2;
        pm.baseProperty.maxHp = 35;
        pm.baseProperty.critRate = 5;
        pm.baseProperty.critDamage = 1.5f;
        pm.baseProperty.maxSpread = 3;
        pm.baseProperty.minSpread = 5;

        pm.baseProperty.critRateCorrection = 1.5f;
        pm.baseProperty.critDamageCorrection = 1.5f;
        
        pm.UpdateProperty();
    }
}