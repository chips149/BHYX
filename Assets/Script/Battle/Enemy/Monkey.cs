using UnityEngine;

public class Monkey : EnemyBase
{
    public ParticleSystem missEffect;
    public override void BeHit(BeHitData data)
    {
        var r = Random.Range(0, 100);
        if (r < 30)
        {
            missEffect.Play();
            return;
        }
        base.BeHit(data);
        if (isDead)
        {
            ani.SetBool("Die", true);
        }
    }
}
