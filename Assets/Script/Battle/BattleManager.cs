using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class BattleManager : IUpdate
{
    private PlayerManager pm;
    private EnemyManager em;

    public Action onDispose;

    // Start is called before the first frame update
    public void Initialize()
    {
        pm = ModulesManager.Get<PlayerManager>();
        pm.Initialize();
        onDispose += ModulesManager.Dispose<PlayerManager>;

        em = ModulesManager.Get<EnemyManager>();
        em.Initialize();
        onDispose += ModulesManager.Dispose<EnemyManager>;


        GameState.Bm = this;
        GameState.Pm = pm;
        GameState.spawnOver = false;

        ModulesManager.Get<GlobalUpdate>().Register(this);

        // 回调赋值
        GameState.onWaveSpawnOver = (count, total) =>
        {
            LevelWaveUI.instance.RefreshUI(GameState.currentLevel, count, total);
            Debug.Log("onWaveEnd");
        };

        GameState.onSpawnComplete = () => { GameState.spawnOver = true; };

        GameState.onLevelClear = () =>
        {
            //TODO:打开卡牌界面
            SpawnMonsterHandler.Instance.drawCardPanel.OpenDrawCardPanel();
        };

        // 开始刷怪
        SpawnMonsterHandler.Instance.StartSpawn();
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