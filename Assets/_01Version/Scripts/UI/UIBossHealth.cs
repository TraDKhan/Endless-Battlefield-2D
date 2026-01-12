using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealth : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    [SerializeField] private TextMeshProUGUI hpText;

    private EnemyHealthController bossHealth;

    public void Bind(EnemyHealthController health)
    {
        // Unbind cũ nếu có
        if (bossHealth != null)
            bossHealth.OnHealthChanged -= UpdateHP;

        bossHealth = health;
        bossHealth.OnHealthChanged += UpdateHP;

        // Update ngay lần đầu
        UpdateHP(bossHealth.CurrentHealth, bossHealth.MaxHealth);
    }

    private void UpdateHP(int current, int max)
    {
        hpFill.fillAmount = (float)current / max;
        hpText.text = $"{current} / {max}";
    }

    private void OnDisable()
    {
        if (bossHealth != null)
            bossHealth.OnHealthChanged -= UpdateHP;
    }
}
