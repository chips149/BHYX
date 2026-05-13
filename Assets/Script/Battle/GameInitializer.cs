using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
       
        var bm = ModulesManager.Get<BattleManager>();
        bm.Initialize();
    }
}
