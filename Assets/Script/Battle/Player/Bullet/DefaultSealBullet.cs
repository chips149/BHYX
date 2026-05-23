using UnityEngine;

public class DefaultSealBullet : MonoBehaviour
{
    public float fallSpeed = 50;
    public float damage;

    [Header("印章子物体")]
    public GameObject sealModel;
    [Header("印章特效")]
    public ParticleSystem spawnEffect;
    public ParticleSystem arrayEffect;
    public ParticleSystem landingEffect;
    public ParticleSystem disappearEffect;

    private static DefaultSealBullet prefab;
    private PlayerManager pm;
    private Vector3 targetPos;
    private int state; 
    private float timer;

    public static DefaultSealBullet Shoot(Vector3 pos, PlayerManager pm, float damage)
    {
        prefab ??= Resources.Load<DefaultSealBullet>("Prefab/Bullet/SealBullet");
        var seal = Instantiate(prefab);
        seal.pm = pm;
        seal.damage = damage;
        seal.transform.position = pos + Vector3.up * 10f;
        seal.targetPos = new Vector3(seal.transform.position.x, 0, seal.transform.position.z);

        seal.sealModel.SetActive(false);
        SoundManager.Play("Audio/SFX/Player/SealSpawn");
        seal.arrayEffect.Play();
        seal.spawnEffect.Play();

        seal.timer = seal.spawnEffect.main.duration-0.4f;
        return seal;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        switch (state)
        {
            case 0: Spawning();  break;
            case 1: Falling();   break;
            case 2: Landed();    break;
            case 3: Disappear(); break;
        }
    }
    private void Spawning()
    {
        if (timer > 0) return;
        sealModel.SetActive(true);
        state = 1;
    }
    
    private void Falling()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, fallSpeed * Time.deltaTime);

        if (transform.position.y > 0.2f) return;
        
        landingEffect.Play();

        var finalDamage = damage;
        Collider[] results = new Collider[32];
        int size = Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.x, results);

        for (int i = 0; i < size; i++)
        {
            var col = results[i];
            if (col.TryGetComponent(out IBeHit hit))
            {
                AttackData attackData = new AttackData { damage = finalDamage };
                pm.container.Execute(attackData);

                hit.BeHit(new BeHitData
                {
                    damage = finalDamage,
                    from = "player",
                    attacker = pm.container,
                    afterHit = AfterHit
                });
            }
        }

        state = 2;
        timer = 0.3f;
    }
    
    private void Landed()
    {
        if (timer > 0) return;
        
        sealModel.SetActive(false);
        disappearEffect.Play();
        SoundManager.Play("Audio/SFX/Player/SealLand");

        state = 3;
        timer = disappearEffect.main.duration;
    }
    
    private void Disappear()
    {
        if (timer > 0) return;

        Destroy(gameObject);
    }

    private void AfterHit(Transform trans, bool isDead)
    {
        if (isDead) CrossDropBullet.Shoot(trans.position + Vector3.up, pm);
    }
}
