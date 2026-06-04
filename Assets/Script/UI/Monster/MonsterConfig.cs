using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MonsterConfig", menuName = "Config/Monster")]
public class MonsterConfig : ScriptableObject
{
    public string prefabPath;
    public List<MonsterInfo> list;
}

[Serializable]
public struct MonsterInfo
{
    public string imgPath;
    public string name;
    public string hp;
    public string trait;
    public string intro;
}