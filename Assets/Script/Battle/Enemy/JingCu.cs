using Framework.Gameplay;
using UnityEngine;

public class JingCu : EnemyBase
{
    public ParticleSystem attackPrefab;
    protected override void Attack(IBeHit beHit)
    {
        attackPrefab.Play();
        pm.container.AddEffect<DotBuff>();
        Destroy(gameObject);
    }
}


