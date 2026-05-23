using System;
using Cysharp.Threading.Tasks;
using Framework;
using Framework.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefaultBullet : MonoBehaviour
{
    public float fixedPeakHeight = 10f;
    public float xzSpeed = 10f;

    public ParticleSystem ballon;
    public ParticleSystem splash;
    private static DefaultBullet prefab;

    private PlayerManager pm;
    private Action<Vector3> onLanded;

    public static DefaultBullet Shoot(Vector3 startPos, Vector3 endPos, PlayerManager pm, Action<Vector3> onLanded = null)
    {
        prefab ??= Resources.Load<DefaultBullet>("Prefab/Bullet/Bullet");
        var bullet = Instantiate(prefab);
        bullet.pm = pm;
        bullet.onLanded = onLanded;
        bullet.transform.localScale *= pm.baseProperty.bulletScale;
        _ = bullet.ShootAni(startPos, endPos);
        return bullet;
    }

    private async UniTaskVoid ShootAni(Vector3 start, Vector3 end)
    {
        float totalDist = Vector3.Distance(start, end);
        float totalTime = totalDist / xzSpeed;

        float y0 = start.y;
        float yEnd = end.y;
        float t = 0;

        while (t < totalTime)
        {
            float k = t / totalTime;

            Vector3 pos = Vector3.Lerp(start, end, k);

            float parabola = 4f * fixedPeakHeight * k * (1f - k);
            pos.y = y0 + (yEnd - y0) * k + parabola;

            transform.position = pos;

            await UniTask.Yield();
            if (this == null) return;
            t += Time.deltaTime;
        }

        transform.position = end;
        onLanded?.Invoke(end);

        // 区域监测
        var finalDamage = pm.GetFinalDamage();
        var results = new Collider[32];
        var size = Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.x, results);
        
        bool hitEnemy = false;
        
        for (var i = 0; i < size; i++)
        {
            var col = results[i];
            var hit = col.gameObject.GetComponent<IBeHit>();
            if (hit == null) continue;
            hitEnemy = true;

            // 先执行攻击buff
            var attackData = new AttackData()
            {
                damage = finalDamage
            };
            pm.container.Execute(attackData);

            // 后执行 behit 的buff
            hit.BeHit(new BeHitData
            {
                damage = finalDamage,
                from = "player",
                attacker = pm.container,
                afterHit = AfterHit
            });
        }

        if (hitEnemy)
        {
            pm.NotifyBulletHit();
            SoundManager.Play("Audio/SFX/Player/BulletHit",0.6f);
        }
        else
        {
            pm.NotifyBulletMiss();
            SoundManager.Play("Audio/SFX/Player/BulletLand",0.6f);
        }

        for (var i = 0; i < size; i++)
        {
            var firePlane = results[i].GetComponent<FirePlane>();
            if (firePlane != null)
            {
                firePlane.DestroyByBullet();
            }
        }

        ballon.gameObject.SetActive(false);
        splash.gameObject.SetActive(true);
        
        Destroy(gameObject,0.3f);
    }

    private void AfterHit(Transform trans, bool isDead)
    {
        if (isDead) CrossDropBullet.Shoot(trans.position + Vector3.up , pm);
    }
}