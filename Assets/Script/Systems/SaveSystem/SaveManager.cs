using System.IO;
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
            if (GameState.Pm.playerHealth != null)
            {
                SaveData.Instance.currentHp = Mathf.RoundToInt(GameState.Pm.playerHealth.currentHp);
            }
        }

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

        ApplyToGameState();
        HasLoadedSave = true;
        return true;
    }

    private static void ApplyToGameState()
    {
        GameState.currentLevel = SaveData.Instance.currentLevel;
        GameState.playerPath = SaveData.Instance.playerPath;
    }
}
