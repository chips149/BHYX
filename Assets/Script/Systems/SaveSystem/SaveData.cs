using System;

[Serializable]
public class SaveData
{
    public static SaveData Instance { get; private set; } = new();

    public int currentLevel = 1;
    public string playerPath = "";
    public int currentHp;

    public PlayerProperty playerProperty = new();

    public static void New()
    {
        Instance = new SaveData();
    }

}
