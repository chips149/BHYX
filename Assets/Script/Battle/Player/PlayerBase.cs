using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using TMPro;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    private static readonly int Aim = Animator.StringToHash("Aim");

    protected AttackHandle atkHandle;
    protected AimHandle aimHandle;
    protected Transform playerPoint;
    protected float attackCooldownTimer;  
    protected bool canAttack = true;   

    private Animator animator;

    [Header("子弹配置")] 
    public int maxBulletCount;
    public float bulletReloadTime = 1.5f;
    public float attackInterval = 0.5f;  

    public int bulletCount;
    private float reloadTimer;
       

    public TextMeshProUGUI text;
    private Vector3 aimPos;

    [Header("Configs")] public float offsetZ = 4;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerPoint = transform.Find("ShootPoint");
    }

    public virtual void Initialize(PlayerManager pm)
    {
        maxBulletCount = pm.baseProperty.maxBulletCount;
        aimHandle = new DefaultAimHandle();
        atkHandle = new DefaultAttackHandle(playerPoint.position, pm);

        aimHandle.onAimEnd = (Vector3 aimPosition) =>
        {
            if (bulletCount > 0 && canAttack)  
            {
                bulletCount--;
                UpdateBulletUI();

                atkHandle.Attack(aimPosition);
                
                canAttack = false;
                attackCooldownTimer = 0;
            }
        };

        var textObj = GameObject.Find("BulletImageText");
        if (textObj != null)
            text = textObj.GetComponent<TextMeshProUGUI>();

        bulletCount = maxBulletCount;
        reloadTimer = 0;
        attackCooldownTimer = 0;
        canAttack = true;
        UpdateBulletUI();
    }

    public void Tick(float dt)
    {
        AutoReloadBullet(dt);
        UpdateAttackCooldown(dt);  

        if (Input.GetMouseButton(0))
        {
            if (bulletCount > 0 && canAttack)  
            {
                aimPos = BattleUtility.AimPosition(transform.position, offsetZ);
                aimHandle.Aiming(aimPos);
                animator.SetBool(Aim, true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            aimHandle.End();
            animator.SetBool(Aim, false);
        }
    }

    private void AutoReloadBullet(float dt)
    {
        if (bulletCount >= maxBulletCount) return;

        reloadTimer += dt;
        if (reloadTimer >= bulletReloadTime)
        {
            bulletCount++;
            reloadTimer = 0;
            UpdateBulletUI();
        }
    }

    public virtual void OnCardDamage(float amount)
    {
        
    }

    private void UpdateAttackCooldown(float dt)
    {
        if (!canAttack)
        {
            attackCooldownTimer += dt;
            if (attackCooldownTimer >= attackInterval)
            {
                canAttack = true;
                attackCooldownTimer = 0;
            }
        }
    }

    public void UpdateBulletUI()
    {
        if (text != null)
        {
            text.text = $"{bulletCount}/{maxBulletCount}";
        }
    }
}

public static class BattleUtility
{
    //计算瞄准点
    public static Vector3 AimPosition(Vector3 origin, float offsetZ)
    {
        // 向量计算
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 摄像机射线
        var len = (origin.y - ray.origin.y) / ray.direction.y; // 射线长度
        var desire = ray.GetPoint(len);
        desire.z += offsetZ;
        desire.y = 0;

        return desire;
    }
}