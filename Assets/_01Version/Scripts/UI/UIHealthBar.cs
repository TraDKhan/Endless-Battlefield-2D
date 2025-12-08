using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI _hpText;

    private PlayerHealthController playerHealth;

    private void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealthController>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealthController not found!");
            return;
        }

        playerHealth.OnHealthChanged += UpdateHealth;

        // Hiển thị lần đầu
        UpdateHealth(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int current, int max)
    {
        if (max <= 0) return; // tránh chia 0

        fillImage.fillAmount = (float)current / max;
        _hpText.text = $"{current} / {max}";
    }
}
