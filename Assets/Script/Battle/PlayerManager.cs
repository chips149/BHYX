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

    public readonly GameplayContainer container = new();

    public void Initialize()
    {
        spawnTrans = GameObject.Find("SpawnPoint").transform;

        var defaultPlayerPrefab = Resources.Load<PlayerBase>("Prefab/Player");
        player = Object.Instantiate(defaultPlayerPrefab, spawnTrans.position, spawnTrans.rotation);
        player.Initialize(this);

        playerHealth = GameObject.Find("Village").GetComponent<PlayerHealth>();
        playerHealth.Initialize(this);

        baseProperty = new PlayerProperty()
        {
            damage = 2,
            critRate = 5,
            critDamage = 1.15f,
            bulletScale = 1f,
            bulletReloadTime = 1.5f,
            maxSpread = 4.5f,
            maxHp = 50f,
        };

        finalProperty = new PlayerProperty()
        {
            damage = baseProperty.damage,
            critRate = baseProperty.critRate,
            critDamage = baseProperty.critDamage,
            bulletScale = baseProperty.bulletScale,
            bulletReloadTime = baseProperty.bulletReloadTime,
            maxSpread = baseProperty.maxSpread,
            maxHp = baseProperty.maxHp,
        };

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
        finalProperty.damage = baseProperty.damage;
        finalProperty.critRate = baseProperty.critRate;
        finalProperty.critDamage = baseProperty.critDamage;
        finalProperty.bulletScale = baseProperty.bulletScale;
        finalProperty.bulletReloadTime = baseProperty.bulletReloadTime;
        finalProperty.maxSpread = baseProperty.maxSpread;
        finalProperty.maxHp = baseProperty.maxHp;

        player.bulletReloadTime = baseProperty.bulletReloadTime;

        playerHealth.maxHp = baseProperty.maxHp;
        if (playerHealth.currentHp > baseProperty.maxHp)
            playerHealth.currentHp = baseProperty.maxHp;
        playerHealth.UpdateHealthDisplay();
        

        container.Execute(finalProperty);
    }

    public void OnDestroy()
    {
    }
}


public class PlayerProperty : GameplayEventData
{
    public float damage;
    public float critRate;
    public float critDamage;
    public float bulletScale = 1f;
    public float bulletReloadTime = 1.5f;
    public float maxSpread = 4.5f;
    public float maxHp = 50f;
}

