using System.Collections.Generic;
using UnityEngine;

public class FirePlane : MonoBehaviour
{
    public float lifeTime = 2f;

    private float timer;
    private readonly HashSet<EnemyBase> enemiesOnPlane = new();
    private float healAccumulator;

    private void Start()
    {
        timer = 0f;
        healAccumulator = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        healAccumulator += Time.deltaTime;
        if (healAccumulator >= 1f)
        {
            healAccumulator -= 1f;
            HealEnemies();
        }
    }

    private void HealEnemies()
    {
        enemiesOnPlane.RemoveWhere(e => e == null || e.isDead);

        foreach (var enemy in enemiesOnPlane)
        {
            if (enemy.hp < enemy.maxHp)
            {
                enemy.hp = Mathf.Min(enemy.hp + 1f, enemy.maxHp);
                enemy.UpdateHealthDisplay();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        var enemy = other.GetComponent<EnemyBase>();
        if (enemy != null && !enemy.isDead)
        {
            enemiesOnPlane.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        var enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemiesOnPlane.Remove(enemy);
        }
    }

    public void DestroyByBullet()
    {
        Destroy(gameObject);
    }
}
