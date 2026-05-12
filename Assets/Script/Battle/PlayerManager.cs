using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

// 管理角色的 生成与销毁
public class PlayerManager
{
    public Transform spawnTrans;
    public PlayerBase player;
    public PlayerHealth playerHealth;
    public PlayerProperty baseProperty;
    public PlayerProperty finalProperty;
    public event Action OnBulletHit;
    public event Action OnBulletMiss;

    public readonly GameplayContainer container = new();
    
    public int bulletCount;
    public float reloadTimer;
    public float attackCooldownTimer;
    public bool canAttack = true;

    public void Initialize()
    {
        spawnTrans = GameObject.Find("SpawnPoint").transform;

        baseProperty = new PlayerProperty()
        {
            damage = 2,
            attackInterval = 1,
            maxBulletCount = 5,
            critRate = 5,
            critDamage = 1.15f,
            bulletScale = 1f,
            bulletReloadTime = 1.5f,
            maxSpread = 4.5f,
            minSpread=2.5f,
            maxHp = 50f,
        };

        finalProperty = new PlayerProperty()
        {
            damage = baseProperty.damage,
            attackInterval = baseProperty.attackInterval,
            maxBulletCount = baseProperty.maxBulletCount,
            critRate = baseProperty.critRate,
            critDamage = baseProperty.critDamage,
            bulletScale = baseProperty.bulletScale,
            bulletReloadTime = baseProperty.bulletReloadTime,
            maxSpread = baseProperty.maxSpread,
            minSpread=baseProperty.minSpread,
            maxHp = baseProperty.maxHp,
        };
        
        playerHealth = GameObject.Find("Village").GetComponent<PlayerHealth>();
        playerHealth.Initialize(this);

        var defaultPlayerPrefab = Resources.Load<PlayerBase>("Prefab/Player");
        player = Object.Instantiate(defaultPlayerPrefab, spawnTrans.position, spawnTrans.rotation);
        player.Initialize(this);
    }

    public void NotifyBulletHit()
    {
        OnBulletHit?.Invoke();
    }

    public void NotifyBulletMiss()
    {
        OnBulletMiss?.Invoke();
    }
    
    public void Tick(float dt)
    {
        player.Tick(Time.deltaTime);

        container.Execute(new FrameData()
        {
            dt = dt,
            beHit = playerHealth,
        });
    }


    public float GetFinalDamage()
    {
        var isCrit = UnityEngine.Random.Range(0, 100) <= baseProperty.critRate;
        var finalDamage = baseProperty.damage * (isCrit ? baseProperty.critDamage : 1);

        return finalDamage;
    }

    public void UpdateProperty()
    {
        finalProperty.damage = baseProperty.damage;//攻击力
        finalProperty.attackInterval = baseProperty.attackInterval;//攻击间隔 
        finalProperty.critRate = baseProperty.critRate;//暴击率 
        finalProperty.critDamage = baseProperty.critDamage;//暴击伤害
        finalProperty.bulletScale = baseProperty.bulletScale; //子弹大小
        finalProperty.bulletReloadTime = baseProperty.bulletReloadTime;//子弹回复速度
        finalProperty.maxSpread = baseProperty.maxSpread;//最大散步范围 
        finalProperty.minSpread = baseProperty.minSpread;//最小散步范围
        finalProperty.maxHp = baseProperty.maxHp;//生命值 

        playerHealth.maxHp = baseProperty.maxHp;
        if (playerHealth.currentHp > baseProperty.maxHp)
            playerHealth.currentHp = baseProperty.maxHp;
        playerHealth.UpdateHealthDisplay();
        
        if (bulletCount > baseProperty.maxBulletCount)
        {
            bulletCount = baseProperty.maxBulletCount;
            player.UpdateBulletUI();
        }

        container.Execute(finalProperty);
    }

    public void OnDestroy()
    {
    }
}


public class PlayerProperty : GameplayEventData
{
    public float damage;//攻击力
    public float attackInterval; //攻击间隔 
    public int maxBulletCount;//子弹上限
    public float critRate;//暴击率
    public float critDamage;//暴击伤害
    public float bulletScale = 1f;//子弹大小
    public float bulletReloadTime = 1.5f;//子弹恢复速度
    public float maxSpread = 4.5f;//最大散步范围
    public float minSpread = 2.5f;//最小散步范围
    public float maxHp = 50f;//血量 

    public float damageCorrection=1;
    public float attackIntervalCorrection=1;
    public float critRateCorrection=1;
    public float critDamageCorrection=1;
}
