using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class CrossDropBullet : MonoBehaviour
{
    private PlayerManager pm;


    public float xzSpeed = 5f;
    private float time;
    public float lifeTime;
    private Vector3 dir;
    
    private static CrossDropBullet prefab;
    private static ObjectPool<CrossDropBullet> pool;
    private static bool isCreated;

    public static void Shoot(Vector3 center, PlayerManager pm)
    {
        if (!isCreated)
        {
            isCreated = true;

            prefab = Resources.Load<CrossDropBullet>("Prefab/Bullet/CrossDrop");
            pool = new ObjectPool<CrossDropBullet>(OnCreate, OnGet, OnRelease);

            GameState.Bm.onDispose += () =>
            {
                pool.Clear();
                pool = null;
                isCreated = false;
                prefab = null;
            };
        }


        ShootAni(center, Vector3.forward);
        ShootAni(center, Vector3.back);
        ShootAni(center, Vector3.right);
        ShootAni(center, Vector3.left);

        return;

        CrossDropBullet OnCreate()
        {
            var bullet = Instantiate(prefab, center, Quaternion.identity);
            bullet.pm = pm;
            return bullet;
        }

        void OnGet(CrossDropBullet bullet)
        {
            bullet.time = 0;
            bullet.gameObject.SetActive(true);
        }

        void OnRelease(CrossDropBullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
    }

    private static void ShootAni(Vector3 center, Vector3 dir)
    {
        var bullet = pool.Get();
        bullet.transform.position = center;
        bullet.dir = dir;
    }


    private void Update()
    {
        transform.position += dir * (Time.deltaTime * xzSpeed);
        time += Time.deltaTime;
        if (time > lifeTime)
            pool.Release(this);
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