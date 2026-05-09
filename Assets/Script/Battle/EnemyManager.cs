using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;


public class EnemyManager
{
    public List<EnemyBase> enemies = new();

    public void Initialize()
    {
        
    }

    public void Register(EnemyBase enemy)
    {
        enemies.Add(enemy);
    }

    public void Unregister(EnemyBase enemy)
    {
        enemies.Remove(enemy);
    }

    public void Tick(float dt)
    {
        foreach (var enemy in enemies)
        {
            enemy.Tick(dt);
        }
    }
}