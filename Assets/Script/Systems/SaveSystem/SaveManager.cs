using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager
{
    private static readonly string SavePath = Application.persistentDataPath + "/save.json";

    public static bool HasLoadedSave { get; private set; }

    public static void ToSave()
    {
        SaveData.Instance.currentLevel = GameState.currentLevel;
        SaveData.Instance.playerPath = GameState.playerPath;

        if (GameState.Pm != null)
        {
            SaveData.Instance.playerProperty = GameState.Pm.baseProperty;
            SaveData.Instance.currentHp = Mathf.RoundToInt(GameState.Pm.playerHealth.currentHp);
            SaveData.Instance.shieldCardLevel = GameState.Pm.playerHealth.shieldCardLevel;
        }

        SaveData.Instance.sessionCoin = CoinSystem.sessionCoin;

        var json = JsonUtility.ToJson(SaveData.Instance, true);
        File.WriteAllText(SavePath, json);
    }

    public static bool Load()
    {
        HasLoadedSave = false;

        if (!File.Exists(SavePath))
        {
            SaveData.New();
            ApplyToGameState();
            return false;
        }

        var json = File.ReadAllText(SavePath);
        if (string.IsNullOrEmpty(json))
        {
            SaveData.New();
            ApplyToGameState();
            return false;
        }

        SaveData.New();
        JsonUtility.FromJsonOverwrite(json, SaveData.Instance);

        if (SaveData.Instance.playerProperty == null)
        {
            SaveData.Instance.playerProperty = new PlayerProperty();
        }

        if (SaveData.Instance.chosenCardIds == null)
        {
            SaveData.Instance.chosenCardIds = new System.Collections.Generic.List<int>();
        }

        ApplyToGameState();
        HasLoadedSave = true;
        return true;
    }

    public static bool HasSaveFile()
    {
        if (!File.Exists(SavePath))
            return false;

        try
        {
            var json = File.ReadAllText(SavePath);
            return !string.IsNullOrWhiteSpace(json);
        }
        catch
        {
            return false;
        }
    }

    public static void ClearPersistedSaveForNewGame()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);

        SaveData.New();
        ApplyToGameState();
        HasLoadedSave = false;
        
        CoinSystem.ResetSession();
        
        WaterTornado.hasWaterTornado = false;
    }
    
    public static void ReplayChosenCards()
    {
        if (SaveData.Instance.chosenCardIds == null || SaveData.Instance.chosenCardIds.Count == 0)
            return;

        var cardMap = CardHandler.Data.ToDictionary(c => c.id, c => c);
        foreach (var cardId in SaveData.Instance.chosenCardIds)
        {
            if (cardMap.TryGetValue(cardId, out var cardData)) 
                cardData.OnReplay();
        }
    }

    private static void ApplyToGameState()
    {
        GameState.currentLevel = SaveData.Instance.currentLevel;
        GameState.playerPath = SaveData.Instance.playerPath;
        CoinSystem.sessionCoin = SaveData.Instance.sessionCoin;
    }
}
