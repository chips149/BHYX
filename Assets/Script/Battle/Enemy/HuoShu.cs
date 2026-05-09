using UnityEngine;
using System.Collections.Generic;

public class HuoShu : EnemyBase
{
    /*public float auraRange = 10f;
    private List<EnemyBase> buffedEnemies = new List<EnemyBase>();

    private protected override void Initialize()
    {
        base.Initialize();
        ApplyAura();
    }

    private void ApplyAura()
    {
        buffedEnemies.Add(this);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, auraRange);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent<EnemyBase>(out var e) && !e.isDead)
            {
                if (!buffedEnemies.Contains(e))
                    buffedEnemies.Add(e);
            }
        }
    }

    public override void Tick(float dt)
    {
        base.Tick(dt);

        if (isDead) return;

        if (buffedEnemies.Contains(this))
        {
            float expectedHp = maxHp - (maxHp - hp) * 0.5f;
            if (hp < expectedHp)
            {
                hp = expectedHp;
                UpdateHealthDisplay();
            }
        }
    }*/
    public override void BeHit(BeHitData data)
    {
        base.BeHit(data);
        if (hp <= 0)
        {
            ani.SetBool("Die", true);
        }
    }
}