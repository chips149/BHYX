using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework{

    public interface IUpdate{
        
        public bool IsDone{get;}
        virtual void OnUpdate(){}
        virtual void OnFixedUpdate(){}
        virtual void OnLateUpdate(){}
    }
    
    
    [ModulesManager.RegisterBeforeSceneLoad]
    public class GlobalUpdate : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
        }

        private readonly List<IUpdate>  _updates = new(); // 所有注册的更新模块

        public void Register(IUpdate update){
            _updates.Add(update); //注册模块
        }

        public void Unregister(IUpdate update){
            _updates.Remove(update); //注销模块
        }

        private void Update(){
            var temp = _updates.ToList();
            foreach (var update in temp){
                update.OnUpdate();
            }
        }

        private void FixedUpdate(){
            var temp = _updates.ToList();
            foreach (var update in temp){
                update.OnFixedUpdate();
            }
        }

        private void LateUpdate(){
            var temp = _updates.ToList();
            temp.ForEach(i=>i.OnLateUpdate());
            
            _updates.RemoveAll(i=>i.IsDone); //清理已完成的模块
        }
    }
}