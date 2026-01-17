using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    [SerializeField] private TextMeshProUGUI hpText;

    private EnemyHealthController health;

    public void Bind(EnemyHealthController target)
    {
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHP;
            health.OnDeath -= HandleDeath;
        }

        health = target;
        health.OnHealthChanged += UpdateHP;
        health.OnDeath += HandleDeath;

        UpdateHP(health.CurrentHealth, health.MaxHealth);
    }

    void UpdateHP(int current, int max)
    {
        hpFill.fillAmount = (float)current / max;
        hpText.text = $"{current} / {max}";
    }

    void HandleDeath()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHP;
            health.OnDeath -= HandleDeath;
        }
    }
}