using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour
{
    [SerializeField] private Image hp_Image;
    [SerializeField] private Image exp_Image;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI timeText;

    private int coinValue;
    private PlayerLevelSystem levelSystem;
    private PlayerHealthController playerHealth;

    //to do init từ player controller
    //private void Start()
    //{
    //    levelSystem = PlayerController.Instance?.LevelSystem;
    //    playerHealth = PlayerController.Instance?.Health;

    //    if (TimeController.Instance != null)
    //        TimeController.Instance.OnTimeChanged += UpdateTime;

    //    if (levelSystem == null)
    //    {
    //        Debug.LogError("UI PlayerLevelSystem not found!");
    //        return;
    //    }

    //    if (playerHealth == null)
    //    {
    //        Debug.LogError("UI PlayerHealthController not found!");
    //        return;
    //    }

    //    levelSystem.OnExpChanged += UpdateExpBar;
    //    levelSystem.OnLevelUp += UpdateLevel;

    //    playerHealth.OnHealthChanged += UpdateHealth;

    //    coinValue = CurrencyManager.Instance.GetCoins();
    //    coinText.text = coinValue.ToString();

    //    // Hiển thị lần đầu
    //    UpdateTime();
    //    UpdateExpBar(levelSystem.CurrentEXP, levelSystem.ExpToNextLevel, levelSystem.CurrentLevel);
    //    UpdateLevel(levelSystem.CurrentLevel);
    //    UpdateHealth(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    //}
    public void Init(PlayerLevelSystem levelSystem, PlayerHealthController playerHealth)
    {
        this.levelSystem = levelSystem;
        this.playerHealth = playerHealth;

        levelSystem.OnExpChanged += UpdateExpBar;
        levelSystem.OnLevelUp += UpdateLevel;
        playerHealth.OnHealthChanged += UpdateHealth;

        if (TimeController.Instance != null)
            TimeController.Instance.OnTimeChanged += UpdateTime;

        if(CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoin;

        coinValue = CurrencyManager.Instance.GetCoins();
        coinText.text = coinValue.ToString();

        // Init UI lần đầu
        UpdateTime();
        UpdateExpBar(levelSystem.CurrentEXP, levelSystem.ExpToNextLevel, levelSystem.CurrentLevel);
        UpdateLevel(levelSystem.CurrentLevel);
        UpdateHealth(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealth;

        if (levelSystem != null)
            levelSystem.OnExpChanged -= UpdateExpBar;

        if (TimeController.Instance != null)
            TimeController.Instance.OnTimeChanged -= UpdateTime;
    }

    private void UpdateHealth(int current, int max)
    {
        if (max <= 0) return;

        hp_Image.fillAmount = (float)current / max;
    }

    private void UpdateExpBar(int currentExp, int expToNext, int level)
    {
        if (expToNext == 0) return;

        exp_Image.fillAmount = (float)currentExp / expToNext;
    }

    private void UpdateLevel(int curentlevel)
    {
        if (curentlevel <= 0) return;
        _levelText.text = $"Level {curentlevel}";
    }

    private void UpdateTime()
    {
        if (TimeController.Instance == null) return;

        timeText.text = TimeController.Instance.GetTimeString();
    }

    private void UpdateCoin(int value)
    {
        coinValue = value;
        coinText.text = coinValue.ToString();
    }
}