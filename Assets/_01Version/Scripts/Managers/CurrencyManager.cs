using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    const string COIN_KEY = "Coin";
    const string GEM_KEY = "Gem";

    int coins;
    int gems;
    public int Coins => coins;

    public event Action<int> OnCoinsChanged;
    public event Action OnCurrencyChanged;

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

    private void Start()
    {
        coins = 1000;
        Debug.Log($"CurrencyManager initialized with {coins} coins and {gems} gems.");
    }

    #region CON HANDLE
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
        OnCurrencyChanged?.Invoke();
    }
    #endregion

    #region GEM HANDLE

    public int GetGems()
    {
        return gems;
    }

    public void AddGems(int amount)
    {
        if (amount == 0) return;
        gems += amount;
        gems = Mathf.Max(0, gems);
        OnCurrencyChanged?.Invoke();
    }
    #endregion

    public void Save()
    {
        PlayerPrefs.SetInt(COIN_KEY, coins);
        PlayerPrefs.SetInt(GEM_KEY, gems);
        PlayerPrefs.Save();
        OnCurrencyChanged?.Invoke();
    }

    public void Load()
    {
        coins = PlayerPrefs.GetInt(COIN_KEY, 0);
        gems = PlayerPrefs.GetInt(GEM_KEY, 1000);
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

    [ContextMenu ("Add 100 Coins")]
    private void Add100Coins()
    {
        AddCoins(100);
    }
}