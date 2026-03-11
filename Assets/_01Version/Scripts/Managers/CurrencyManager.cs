using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    const string COIN_KEY = "Coin";

    int coins;
    public int Coins => coins;

    public event Action<int> OnCoinsChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public int GetCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        if (amount == 0) return;

        coins += amount;
        coins = Mathf.Max(0, coins);

        OnCoinsChanged?.Invoke(coins);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(COIN_KEY, coins);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        coins = PlayerPrefs.GetInt(COIN_KEY, 0);
        OnCoinsChanged?.Invoke(coins);
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            Save();
    }
}