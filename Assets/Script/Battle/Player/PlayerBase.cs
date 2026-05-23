using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerBase : MonoBehaviour
{
    private static readonly int Aim = Animator.StringToHash("Aim");

    protected PlayerManager pm;
    protected AttackHandle atkHandle;
    protected AimHandle aimHandle;
    protected Transform playerPoint;

    private Animator animator;
    private TextMeshProUGUI text;
    private Vector3 aimPos;

    [Header("Configs")] public float offsetZ = 4;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerPoint = transform.Find("ShootPoint");


        var textObj = GameObject.Find("BulletImageText");
        if (textObj != null)
            text = textObj.GetComponent<TextMeshProUGUI>();
    }

    public virtual void Initialize(PlayerManager pm)
    {
        this.pm = pm;
        aimHandle = new DefaultAimHandle();
        atkHandle = new DefaultAttackHandle(playerPoint.position, pm);

        pm.bulletCount = pm.baseProperty.maxBulletCount;
        pm.reloadTimer = 0;
        pm.attackCooldownTimer = 0;
        pm.canAttack = true;
        UpdateBulletUI();
    }

    public void Tick(float dt)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        AutoReloadBullet(dt);
        UpdateAttackCooldown(dt);

        if (Input.GetMouseButton(0))
        {
            if (pm.bulletCount > 0 && pm.canAttack)
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
        if (pm.bulletCount >= pm.baseProperty.maxBulletCount) return;

        pm.reloadTimer += dt;
        if (pm.reloadTimer >= pm.baseProperty.bulletReloadTime)
        {
            pm.bulletCount++;
            pm.reloadTimer = 0;
            UpdateBulletUI();
        }
    }

    public virtual void OnCardDamage(float amount)
    {
    }

    private void UpdateAttackCooldown(float dt)
    {
        if (!pm.canAttack)
        {
            pm.attackCooldownTimer += dt;
            if (pm.attackCooldownTimer >= pm.baseProperty.attackInterval)
            {
                pm.canAttack = true;
                pm.attackCooldownTimer = 0;
            }
        }
    }

    public void UpdateBulletUI()
    {
        text.text = $"{pm.bulletCount}/{pm.baseProperty.maxBulletCount}";
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