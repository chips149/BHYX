using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MagicWeaponConfig", menuName = "Config/MagicWeapon")]
public class MagicWeaponConfig : ScriptableObject
{
    public string prefabPath;
    public List<MagicWeaponInfo> list;
}

[Serializable]
public struct MagicWeaponInfo
{
    public string imgPath;
    public string name;
    public string description;
    public string boostAdjust;
}


