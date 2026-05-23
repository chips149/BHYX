using Framework.Gameplay;
using UnityEngine;

public class JingCu : EnemyBase
{
    public ParticleSystem attackPrefab;
    public GameObject jingCuPre;
    protected override void Attack(IBeHit beHit)
    {
        isDead = true; 
        jingCuPre.SetActive(false);
        attackPrefab.Play();
        pm.container.AddEffect<DotBuff>();
        Destroy(gameObject, 0.4f);
    }

}


