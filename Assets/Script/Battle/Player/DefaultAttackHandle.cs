using System;
using UnityEngine;

public abstract class AttackHandle
{
    public abstract void Attack(Vector3 aimPos);
}

public class DefaultAttackHandle : AttackHandle
{
    private readonly Vector3 startPos;
    private readonly PlayerManager pm;

    public DefaultAttackHandle(Vector3 startPos, PlayerManager pm)
    {
        this.startPos = startPos;
        this.pm = pm;
    }

    public override void Attack(Vector3 aimPos)
    {
        DefaultBullet.Shoot(startPos, aimPos, pm);
    }
}
