// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// // 客户端调用
// public class StateMachine
// {
//     private readonly Dictionary<Enum, StateBase> stateDic = new();
//     public StateBase Current { get; private set; }
//
//     public void RegisterState(Enum key, StateBase state)
//     {
//         if (stateDic.TryAdd(key, state))
//         {
//             state.machine = this;
//         }
//     }
//
//     public void UnregisterState(Enum key)
//     {
//         stateDic.Remove(key);
//     }
//
//     public void To(Enum key)
//     {
//         Current.OnExit();
//         Current = stateDic[key];
//         Current.OnEnter();
//     }
//
//
//     public void Tick(float dt)
//     {
//         Current?.OnTick(dt);
//     }
//
// }
//
//
//
//
// public abstract class StateBase
// {
//     public StateMachine machine;
//     public abstract void OnEnter();
//     public abstract void OnTick(float dt);
//     public abstract void OnExit();
// }