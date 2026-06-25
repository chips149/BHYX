using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CrossDropBullet : MonoBehaviour
{
    private PlayerManager pm;

    public float xzSpeed = 5f;
    private float time;
    public float lifeTime;
    private Vector3 dir;
    
    private static CrossDropBullet prefab;
    private static Stack<CrossDropBullet> pool;
    private static bool isCreated;

    public static void Shoot(Vector3 center, PlayerManager pm)
    {
        if (!isCreated)
        {
            isCreated = true;
            prefab = Resources.Load<CrossDropBullet>("Prefab/Bullet/CrossDrop");
            pool = new Stack<CrossDropBullet>();

            GameState.Bm.onDispose += () =>
            {
                pool.Clear();
                pool = null;
                isCreated = false;
                prefab = null;
            };
        }

        ShootAni(center, Vector3.forward, pm);
        ShootAni(center, Vector3.back, pm);
        ShootAni(center, Vector3.right, pm);
        ShootAni(center, Vector3.left, pm);
    }

    private static void ShootAni(Vector3 center, Vector3 dir, PlayerManager pm)
    {
        var bullet = GetFromPool();
        bullet.pm = pm;
        bullet.transform.position = center;
        bullet.dir = dir;
        bullet.time = 0;
        bullet.gameObject.SetActive(true);
    }

    private static CrossDropBullet GetFromPool()
    {
        while (pool != null && pool.Count > 0)
        {
            var bullet = pool.Pop();
            if (bullet != null)
                return bullet;
            // 跳过已销毁的引用
        }
        return Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    public void Release()
    {
        if (this == null || pool == null) return;
        gameObject.SetActive(false);
        pool.Push(this);
    }

    private void Update()
    {
        transform.position += dir * (Time.deltaTime * xzSpeed);
        time += Time.deltaTime;
        if (time > lifeTime)
            Release();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (!other.TryGetComponent<IBeHit>(out var hit)) return;

        hit.BeHit(new BeHitData
        {
            damage = pm.GetFinalDamage(),
            from = "player",
            attacker = pm.container,
            afterHit = AfterHit
        });
    }

    private void AfterHit(Transform trans, bool isDead)
    {
        if (isDead) Shoot(trans.position, pm);
    }
}