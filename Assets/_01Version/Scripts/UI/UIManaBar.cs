using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManaBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text manaText;

    private PlayerManaController mana;

    private void Start()
    {
        var player = PlayerController.Instance;
        if (player == null) return;

        mana = player.Mana;
        if (mana == null) return;

        mana.OnMPChanged += UpdateUI;

        // Update lần đầu
        UpdateUI(mana.CurrentMP, mana.MaxMP);
    }

    private void OnDestroy()
    {
        if (mana != null)
            mana.OnMPChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
        float percent = max > 0 ? (float)current / max : 0f;

        if (fillImage != null)
            fillImage.fillAmount = percent;

        if (manaText != null)
            manaText.text = $"{current} / {max}";
    }
}
