using System;
using UnityEngine;

public static class Dev3GameContext
{
    public static readonly CurrencyService Currency = new CurrencyService();
    public static readonly DataService Data = new DataService();
}

public class CurrencyService
{
    private const string CoinKey = "PLAYER_COINS";

    public event Action<int> OnCoinChanged;

    public int GetCoins()
    {
        if (!PlayerPrefs.HasKey(CoinKey))
        {
            PlayerPrefs.SetInt(CoinKey, 1000);
            PlayerPrefs.Save();
        }

        return PlayerPrefs.GetInt(CoinKey);
    }

    public bool TrySpendCoins(int amount)
    {
        int currentCoins = GetCoins();

        if (amount <= 0)
        {
            Debug.LogWarning("Số tiền nâng cấp không hợp lệ.");
            return false;
        }

        if (currentCoins < amount)
        {
            Debug.Log("Không đủ tiền để nâng cấp.");
            return false;
        }

        int newCoins = currentCoins - amount;
        PlayerPrefs.SetInt(CoinKey, newCoins);
        PlayerPrefs.Save();

        OnCoinChanged?.Invoke(newCoins);
        return true;
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        int newCoins = GetCoins() + amount;
        PlayerPrefs.SetInt(CoinKey, newCoins);
        PlayerPrefs.Save();

        OnCoinChanged?.Invoke(newCoins);
    }
}

public class DataService
{
    public int GetBuildingLevel(string buildingId)
    {
        string key = GetBuildingLevelKey(buildingId);
        return PlayerPrefs.GetInt(key, 0);
    }

    public void SaveBuildingLevel(string buildingId, int level)
    {
        string key = GetBuildingLevelKey(buildingId);
        PlayerPrefs.SetInt(key, level);
        PlayerPrefs.Save();
    }

    private string GetBuildingLevelKey(string buildingId)
    {
        return "BUILDING_LEVEL_" + buildingId;
    }
}