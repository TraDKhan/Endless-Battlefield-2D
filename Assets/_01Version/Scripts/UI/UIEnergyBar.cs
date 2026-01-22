using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEnergyBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text energyText;

    private PlayerEnergyController energy;

    private void Start()
    {
        var player = PlayerController.Instance;
        if (player == null) return;

        energy = player.Energy;
        if (energy == null) return;

        energy.OnEnergyChanged += UpdateUI;

        // update lần đầu
        UpdateUI(
            energy.CurrentEnergy,
            player.Stats.GetMaxEnergy()
        );
    }

    private void OnDestroy()
    {
        if (energy != null)
            energy.OnEnergyChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
        float percent = max > 0 ? (float)current / max : 0f;

        if (fillImage != null)
            fillImage.fillAmount = percent;

        if (energyText != null)
            energyText.text = $"{current} / {max}";
    }
}
