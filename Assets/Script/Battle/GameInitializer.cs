using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Load();

        var bm = ModulesManager.Get<BattleManager>();
        bm.Initialize();

        // 读档完后按当前关卡加载对应环境
        EnvironmentManager.Instance.Init();
    }

    private void OnDestroy()
    {
        ModulesManager.Dispose<BattleManager>();
    }
}
