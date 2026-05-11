using System;
using Framework;
using Framework.Gameplay;
using TMPro;
using UnityEngine;


// 目的：
// 1. 用于管理
// 2. 用来写状态机
public abstract class EnemyBase : MonoBehaviour, IBeHit
{
    public float hp = 2;
    public float maxHp = 2;
    public TextMeshProUGUI enemyText;

    public bool isDead;
    public float speed = 1;
    public float distance = 2;

    public ParticleSystem beHitPrefab;
    protected Animator ani;
    protected PlayerManager pm;

    public readonly GameplayContainer container = new();

    private void Awake()
    {
        maxHp = hp;
        UpdateHealthDisplay();
        
        pm = ModulesManager.Get<PlayerManager>();
        ani = GetComponent<Animator>();
        // 初始化
        Initialize();
    }

    private void OnEnable() => ModulesManager.Get<EnemyManager>().Register(this);
    private void OnDisable() => ModulesManager.Get<EnemyManager>().Unregister(this);

    private protected virtual void Initialize()
    {
    }

    public virtual void Tick(float dt)
    {
        if (isDead) return;

        var d = Mathf.Abs(pm.playerHealth.transform.position.z - transform.position.z);
        if (d < distance)
        {
            Attack(pm.playerHealth);
        }
        else
        {
            Move(dt);
        }
    }

    protected virtual void Attack(IBeHit beHit)
    {
        ani.SetBool("ATK", true);
        //... 死亡效果
        isDead = true;
        
        beHit.BeHit(new()
        {
            damage = hp,
            from = name
        });
    }

    protected virtual void Move(float dt)
    {
        transform.position += transform.forward * (speed * dt);
    }
    
    
    // default
    public virtual void BeHit(BeHitData data)
    {
        if (isDead) return;
        container.Execute(data);

        hp -= data.damage;
        beHitPrefab.Stop();
        beHitPrefab.Play();
        data.afterHit?.Invoke(transform, hp <= 0);
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            data.attacker?.Execute(new KillData()
            {
                beKiller = transform,
            });
            Destroy(gameObject, 0.4f);
        }
        UpdateHealthDisplay();
    }

    public void RemoveHp(RemoveHpData data)
    {
        container.Execute(data);
        TakeDamage(data.damage);
    }


    private void TakeDamage(float damage)
    {
        if (isDead) return;
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            // 播放死亡动画
            // ani.SetBool("DEAD", true);
            Destroy(gameObject);
        }
    }

    // 帧事件
    public virtual void Disappear()
    {
        isDead = true;
        Destroy(gameObject);
    }


    public void UpdateHealthDisplay()
    {
        enemyText.text = $"{hp}";
    }
}