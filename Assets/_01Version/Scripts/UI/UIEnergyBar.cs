using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEnergyBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text energyText;

    private void Start()
    {
        if (PlayerEnergyController.Instance != null)
        {
            PlayerEnergyController.Instance.OnEnergyChanged += UpdateUI;

            // update lần đầu
            UpdateUI(
                PlayerEnergyController.Instance.CurrentEnergy,
                GetMaxEnergy()
            );
        }
    }

    private void OnDestroy()
    {
        if (PlayerEnergyController.Instance != null)
            PlayerEnergyController.Instance.OnEnergyChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
        float percent = max > 0 ? (float)current / max : 0f;

        if (fillImage != null)
            fillImage.fillAmount = percent;

        if (energyText != null)
            energyText.text = $"{current} / {max}";
    }

    private int GetMaxEnergy()
    {
        var stats = CharacterStatsController.Instance?.Stats;
        return stats != null ? stats.GetMaxEnergy() : 0;
    }
}
