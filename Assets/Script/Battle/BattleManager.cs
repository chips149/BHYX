using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

//[RegisterBeforeSceneLoad]
public class BattleManager : IUpdate
{
    private readonly PlayerManager pm;
    private readonly EnemyManager em;

    public  Action onDispose;

    // Start is called before the first frame update
    public BattleManager()
    {
        pm = ModulesManager.Get<PlayerManager>();
        pm.Initialize();
        onDispose += ModulesManager.Dispose<PlayerManager>;

        em = ModulesManager.Get<EnemyManager>();
        em.Initialize();
        onDispose += ModulesManager.Dispose<EnemyManager>;


        GameState.Bm = this;
        GameState.Pm = pm;

        ModulesManager.Get<GlobalUpdate>().Register(this);
    }

    public bool IsDone { get; set; }

    public void OnUpdate()
    {
        var dt = Time.deltaTime;
        pm.Tick(dt);
        em.Tick(dt);
    }

    public void OnDestroy()
    {
        onDispose();
    }
}