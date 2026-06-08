using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public static SaveData Instance { get; private set; } = new();

    public int currentLevel = 1;
    public string playerPath = "";
    public int currentHp;
    public int shieldCardLevel;
    public int sessionCoin;

    public PlayerProperty playerProperty = new();
    public List<int> chosenCardIds = new();

    public static void New()
    {
        Instance = new SaveData();
    }

    public static void AddCard(int cardId)
    {
        Instance.chosenCardIds.Add(cardId);
    }
}
