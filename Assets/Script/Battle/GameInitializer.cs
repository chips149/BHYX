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
    }

    private void OnDestroy()
    {
        ModulesManager.Dispose<BattleManager>();
    }
}
