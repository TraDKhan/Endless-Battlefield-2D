using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _levelText;

    private PlayerLevelSystem levelSystem;

    private void Start()
    {
        levelSystem = FindAnyObjectByType<PlayerLevelSystem>();

        if (levelSystem == null)
        {
            Debug.LogError("PlayerLevelSystem not found!");
            return;
        }

        levelSystem.OnExpChanged += UpdateExpBar;
        levelSystem.OnLevelUp += UpdateLevel;

        UpdateExpBar(levelSystem.CurrentEXP, levelSystem.ExpToNextLevel, levelSystem.CurrentLevel);
        UpdateLevel(levelSystem.CurrentLevel);
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
            levelSystem.OnExpChanged -= UpdateExpBar;
    }

    private void UpdateExpBar(int currentExp, int expToNext, int level)
    {
        if (expToNext == 0) return;

        fillImage.fillAmount = (float)currentExp / expToNext;
        _expText.text = $"{currentExp} / {expToNext}";
    }

    private void UpdateLevel(int curentlevel)
    {
        if(curentlevel <= 0) return;
        _levelText.text = $"Level {curentlevel}";
    }
}
