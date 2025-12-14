using System.Collections.Generic;
using UnityEngine;

public class UIUpgradePanel : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Upgrade option slots")]
    public UIUpgradeOption[] optionSlots;

    private List<UpgradeData> currentOptions;

    private PlayerUpgradeSystem upgradeSystem;

    private void Start()
    {
        upgradeSystem = PlayerUpgradeSystem.Instance;

        if (upgradeSystem != null)
            upgradeSystem.OnShowUpgradeUI += Show;

        Hide();
    }

    private void OnDestroy()
    {
        if (upgradeSystem != null)
            upgradeSystem.OnShowUpgradeUI -= Show;
    }

    // =========================
    // SHOW UI
    // =========================
    private void Show(List<UpgradeData> options)
    {
        currentOptions = options;
        panelRoot.SetActive(true);

        // Pause game
        Time.timeScale = 0f;

        for (int i = 0; i < optionSlots.Length; i++)
        {
            if (i >= options.Count)
            {
                optionSlots[i].gameObject.SetActive(false);
                continue;
            }

            optionSlots[i].gameObject.SetActive(true);

            var upgrade = options[i];
            int index = i;

            // ✅ SET DATA (LOGIC MỚI)
            optionSlots[i].SetData(upgrade, upgradeSystem);

            // ✅ BIND BUTTON
            optionSlots[i].BindButton(() =>
            {
                upgradeSystem.ApplyUpgrade(currentOptions[index]);
                Hide();
            });
        }
    }

    // =========================
    // HIDE UI
    // =========================
    public void Hide()
    {
        panelRoot.SetActive(false);
        Time.timeScale = 1f;
    }
}
