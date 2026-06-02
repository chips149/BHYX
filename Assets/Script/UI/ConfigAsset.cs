using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Custom Config",menuName = "")]
public class ConfigAsset : ScriptableObject
{
    public string prefabPath;
    public List<ConfigInfo> list;
}

[Serializable]
public struct ConfigInfo
{
    public string imgPath;
    public string name;
    public string description;
    public string boostAdjust;
}
