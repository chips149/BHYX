using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Framework.Gameplay
{
    // public static class GameplayCombineHandle
    // {
    //     public static void CombineExecute<T>(T data, params GameplayContainer[] containers) where T : GameplayEventData
    //     {
    //         var combine = new GameplayContainer();
    //         foreach (var container in containers)
    //         {
    //             combine.effects.AddRange(container.effects);
    //         }
    //
    //         combine.Build();
    //
    //         combine.Execute(data);
    //     }
    // }

    // 容器
    public class GameplayContainer
    {
        private readonly List<GameplayEffect> effects = new();

        public void Build()
        {
            // 排序
            effects.Sort((a, b) => a.sort = b.sort);
        }

        public void AddEffect<T>() where T : GameplayEffect
        {
            if (effects.Exists(effect => effect is T))
            {
                var e = effects.OfType<T>().First();
                if (e.canSolo)
                {
                    CreateEffect();
                }
                else
                {
                    e.OnRefresh();
                }

                return;
            }

            CreateEffect();
            return;

            void CreateEffect()
            {
                var newEffect = Activator.CreateInstance<T>();
                effects.Add(newEffect);
            }
        }

        public void RemoveEffect<T>(int count = 1) where T : GameplayEffect
        {
            var es = effects.FindAll(e => e is T);

            es.GetRange(0, count < es.Count ? count : es.Count)
                .ForEach(e =>
                {
                    e.OnRemove();
                    effects.Remove(e);
                });
        }


        public void Execute<T>(T ctx) where T : GameplayEventData
        {
            var temp = effects.OfType<IGameplayEvent<T>>().ToArray();
            foreach (var effect in temp)
            {
                effect.Execute(ctx);

                if (ctx.isInterrupt)
                    return;
            }

            foreach (var effect in temp)
            {
                if (effect is not GameplayEffect { finish: true } e) continue;
                e.OnRemove();
                effects.Remove(e);
            }
        }
    }

    public abstract class GameplayEffect
    {
        public int sort;
        public bool canSolo = false;
        public bool finish = false;

        public virtual void OnRefresh()
        {
        }

        public virtual void OnRemove()
        {
        }
    }

    public interface IGameplayEvent<in T> where T : GameplayEventData
    {
        void Execute(T data);
    }

    public abstract class GameplayEventData
    {
        // 流程控制
        public bool isInterrupt = false;
    }

    //----------------------------------------------------------------------
    // 定义事件的data

    //
    // public class Attacking : GameplayEventData
    // {
    //     public IAttackAbility a;
    //     public IBeHitable b;
    //     public float baseValue;
    //     public float moreValue;
    // }
    //
    // // 定义 Buff
    // public class DoubleDamage : GameplayEffect, IGameplayEvent<Attacking>
    // {
    //     public void Execute(Attacking data)
    //     {
    //         data.moreValue += data.baseValue;
    //     }
    // }
    //
    // // 调用
    // public class Player
    // {
    //     private GameplayContainer container;
    //
    //     void Attacking()
    //     {
    //         GameplayCombineHandle.CombineExecute(new Attacking(), this.container, this.container, this.container);
    //     }
    // }
}