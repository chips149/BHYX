using System;
using Framework.Gameplay;
using UnityEngine;

public class FrameData : GameplayEventData
{
    public float dt;
    public IBeHit beHit;
}

public class AttackData : GameplayEventData
{
    public float damage;
}

public class BeHitData : GameplayEventData
{
    public float damage;
    public string from;
    public GameplayContainer attacker;
    public Action<Transform, bool> afterHit;
}
public class RemoveHpData : GameplayEventData
{
    public float damage;
    public string from;
}

public class AfterAttack : GameplayEventData
{
    public Transform beAttacker;
}

public class KillData : GameplayEventData
{
    public Transform beKiller;
}

public class DamageReduceEffect: GameplayEventData
{
}

