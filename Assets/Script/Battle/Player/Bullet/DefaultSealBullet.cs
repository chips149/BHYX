using Cysharp.Threading.Tasks;
using UnityEngine;

public class DefaultSealBullet : MonoBehaviour
{
    public float fallSpeed = 10;
    public float damage;

    private static DefaultSealBullet prefab;
    private PlayerManager pm;

    public static DefaultSealBullet Shoot(Vector3 pos, PlayerManager pm, float damage)
    {
        prefab ??= Resources.Load<DefaultSealBullet>("Prefab/Bullet/SealBullet");
        var seal = Instantiate(prefab);
        seal.pm = pm;
        seal.damage = damage;
        seal.transform.position = pos + Vector3.up * 10f; 
        _ = seal.FallAni();
        return seal;
    }

    private async UniTaskVoid FallAni()
    {
        Vector3 targetPos = new Vector3(transform.position.x, 0, transform.position.z);

        while (transform.position.y > 0.2f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, fallSpeed * Time.deltaTime);
            await UniTask.Yield();
        }

        
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

        Destroy(gameObject, 0.5f);
    }
    private void AfterHit(Transform trans, bool isDead)
    {
        if (isDead) CrossDropBullet.Shoot(trans.position + Vector3.up , pm);
    }
}