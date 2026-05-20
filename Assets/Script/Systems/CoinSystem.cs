using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinSystem
{
    private const string Key = "PlayerCoin";

    public static int sessionCoin;
    
    public static int GetCoin()
    {
        return PlayerPrefs.GetInt(Key, 0);
    }

    public static void AddCoin(int amount)
    {
        sessionCoin += amount;
        PlayerPrefs.SetInt(Key,GetCoin()+amount);
        PlayerPrefs.Save();
    }

    public static bool SpendCoin(int amount)
    {
        int current = GetCoin();
        if(current<amount)return false; 
        
        PlayerPrefs.SetInt(Key,current-amount);
        PlayerPrefs.Save();
        return true;
    }
    
    public static void Reset()
    {
        PlayerPrefs.DeleteKey(Key);
    }
}
