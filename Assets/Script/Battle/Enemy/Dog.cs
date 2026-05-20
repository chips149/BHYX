using UnityEngine;

public class Dog : EnemyBase
{
    private static GameObject firePlane;
    public ParticleSystem atkEffect;
    public GameObject enemyPrefab;
    
    [Header("生成间隔")]
    public float spawnInterval = 1.5f; 
    private float spawnTimer; 

    void Start()
    {
        firePlane ??= Resources.Load<GameObject>("Prefab/Enemy/FirePlane");
        spawnTimer = 0;

        if (GameState.Bm != null)
        {
            GameState.Bm.onDispose += () => firePlane = null;
        }
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

    protected override void Move(float dt)
    {
        transform.position += transform.forward * (speed * dt);

        spawnTimer += dt;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            Instantiate(firePlane, transform.position, transform.rotation);
        }
    }
}