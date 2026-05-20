using UnityEngine;

public class Monkey : EnemyBase
{
    public ParticleSystem missEffect;
    public ParticleSystem atkEffect;
    public GameObject enemyPrefab;
    
    public void Atk()
    {
        enemyPrefab.SetActive(false);
        atkEffect.Play();
    }
    
    
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
