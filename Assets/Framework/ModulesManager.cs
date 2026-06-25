using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using Object = UnityEngine.Object;


namespace Framework
{
    public static class ModulesManager
    {
        private static readonly Dictionary<string, object> MODULS = new();
        
        //扫描所有程序集，自动创建带 [RegisterBeforeSceneLoad] 的模块实例
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 只扫描主程序集
            var targetAssemblies = assemblies.Where(_assembly =>
                _assembly.FullName.StartsWith("Assembly-CSharp") ||
                _assembly.FullName.StartsWith("Assembly-CSharp-firstpass"));

            var types = targetAssemblies
                .SelectMany(_assembly => _assembly.GetTypes())
                .Where(Predicate);

            foreach (var type in types)
            {
                MODULS.TryAdd(type.Name, CreateInstance(type)); // 注册到字典
            }

            return;

            //有 [RegisterBeforeSceneLoad] 且是非抽象类
            bool Predicate(Type _type)
            {
                return _type is not null &&
                       Attribute.IsDefined(_type, typeof(RegisterBeforeSceneLoad)) &&
                       !_type.IsAbstract &&
                       _type.IsClass &&
                       _type.FullName != null &&
                       !_type.FullName.Contains("+");
            }
        }



        //MonoBehaviour：从场景查找或新建 GameObject
        //ScriptableObject：直接创建或从 Resources 加载
        //普通类：Activator 直接 new
        private static object CreateInstance(Type _type)
        {
            if (_type.IsSubclassOf(typeof(MonoBehaviour)))
                return CreateMonoInstance(_type);

            if (_type.IsSubclassOf(typeof(ScriptableObject)))
                return CreatScriptableInstance(_type);

            //     return Activator.CreateInstance(type) as IModul;
            return _type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
        }
        
        private static object CreateMonoInstance(Type _type)
        {
            var comp = Object.FindObjectOfType(_type);

            if (comp != null) return comp; // 场景中已有

            var loader = _type.GetCustomAttribute<LoadInsteadOf>();
            if (loader is null)
                return new GameObject(_type.Name).AddComponent(_type); // 创建新物体

            var prefab = Resources.Load(loader.PATH, _type); // 从 Resources 加载预制体
            var obj = Object.Instantiate(prefab);
            obj.name = prefab.name;
            return obj;
        }


        private static object CreatScriptableInstance(Type _type)
        {
            var loader = _type.GetCustomAttribute<LoadInsteadOf>();
            if (loader is null)
                return ScriptableObject.CreateInstance(_type);

            return Resources.Load(loader.PATH, _type); // 从 Resources 加载
        }



        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (MODULS.TryGetValue(type.Name, out var modul))
            {
                if (modul == null)
                {
                    MODULS[type.Name] = modul = CreateInstance(type); // 懒加载重建
                }

                return modul as T;
            }


            var newModul = CreateInstance(type); // 首次创建
            MODULS.Add(type.Name, newModul);

            return newModul as T;
        }


        public static void Dispose<T>() where T : class
        {
            var type = typeof(T);
            if (MODULS.TryGetValue(type.Name, out var modul))
            {
                if (modul is IDisposable disposable)
                    disposable.Dispose();
                MODULS.Remove(type.Name);
            }
        }


        /// <summary>
        /// 一上来就需要加载的
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public class RegisterBeforeSceneLoad : Attribute
        {
        }

        /// <summary>
        /// 一个标识符 用来 标记某个需要读取资源来创建， 而不是创建一个新的
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public class LoadInsteadOf : Attribute
        {
            public readonly string PATH;

            public LoadInsteadOf(string _path)
            {
                PATH = _path;
            }
        }
    }
}