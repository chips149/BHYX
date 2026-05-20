using UnityEngine;

public class Phoenix : EnemyBase
{
    public ParticleSystem rushPrefab;
    public ParticleSystem atkEffect;
    public GameObject enemyPrefab;
    
    public void Rush()
    {
        rushPrefab.gameObject.SetActive(true);
    }
    
    public void Atk()
    {
        enemyPrefab.SetActive(false);
        atkEffect.Play();
    }
    
    public override void BeHit(BeHitData data)
    {
        base.BeHit(data);
        if (hp <= 0)
        {
            ani.SetBool("Die", true);
        }
    }
}
