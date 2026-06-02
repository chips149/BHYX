using System.Collections.Generic;
using UnityEngine;

public class YuHuanPlayer : PlayerBase
{
    private float originalDamage;
    private List<BoostOrb> orbs = new();

    public override void Initialize(PlayerManager pm)
    {
        base.Initialize(pm);

        pm.baseProperty = MagicWeaponLevelUpSystem.GetBaseProperty("YuHuan");
        int level = MagicWeaponLevelUpSystem.GetLevel("YuHuan");
        MagicWeaponLevelUpSystem.ApplyUpLevel(pm.baseProperty, level);

        originalDamage = pm.baseProperty.damage;

        pm.UpdateProperty();

        pm.OnBulletHit += OnHit;
        pm.OnBulletMiss += OnMiss;

        aimHandle.onAimEnd = (aimPosition) =>
        {
            if (pm.bulletCount > 0 && pm.canAttack)
            {
                pm.bulletCount--;
                UpdateBulletUI();

                atkHandle.Attack(aimPosition);

                pm.canAttack = false;
                pm.attackCooldownTimer = 0;
            }
        };
    }

    public override void OnCardDamage(float amount)
    {
        originalDamage += amount;
        pm.baseProperty.damage = originalDamage;
        pm.UpdateProperty();
    }

    private void OnHit()
    {
        pm.baseProperty.damage = originalDamage;
        pm.UpdateProperty();

        foreach (var orb in orbs)
            if (orb != null) Destroy(orb.gameObject);
        orbs.Clear();
    }

    private void OnMiss()
    {
        pm.baseProperty.damage += 1;
        pm.UpdateProperty();

        if (playerPoint != null)
        {
            int count = orbs.Count + 1;
            float spacing = 360f / count;

            for (int i = 0; i < orbs.Count; i++)
            {
                if (orbs[i] != null)
                {
                    orbs[i].SetTotalCount(count);
                    orbs[i].SetBaseAngle(i * spacing);
                }
            }

            float angle = orbs.Count * spacing;
            var newOrb = BoostOrb.Create(playerPoint, pm.lastAimPos, angle, count);
            newOrb.OnOrbDestroyed = () =>
            {
                pm.baseProperty.damage = originalDamage;
                pm.UpdateProperty();
                orbs.Remove(newOrb);
                int newCount = orbs.Count;
                float newSpacing = 360f / newCount;
                for (int i = 0; i < orbs.Count; i++)
                {
                    if (orbs[i] != null)
                    {
                        orbs[i].SetTotalCount(newCount);
                        orbs[i].SetBaseAngle(i * newSpacing);
                    }
                }
            };
            orbs.Add(newOrb);
        }
    }

    private void OnDestroy()
    {
        pm.OnBulletHit -= OnHit;
        pm.OnBulletMiss -= OnMiss;
    }
}
