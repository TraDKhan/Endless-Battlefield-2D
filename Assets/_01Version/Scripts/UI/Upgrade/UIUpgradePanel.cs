using System.Collections.Generic;
using UnityEngine;

public class UIUpgradePanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelRoot;

    [Header("Upgrade option slots")]
    [SerializeField] private UIUpgradeOption[] optionSlots;

    private UpgradeSystem upgradeSystem;

    private void Start()
    {
        upgradeSystem = UpgradeSystem.Instance;

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
    // SHOW
    // =========================
    private void Show(List<UpgradeData> options)
    {
        panelRoot.SetActive(true);
        Time.timeScale = 0f;

        for (int i = 0; i < optionSlots.Length; i++)
        {
            if (i >= options.Count)
            {
                optionSlots[i].gameObject.SetActive(false);
                continue;
            }

            optionSlots[i].gameObject.SetActive(true);

            UpgradeData upgrade = options[i];

            optionSlots[i].SetData(
                upgrade,
                upgradeSystem,
                () =>
                {
                    upgradeSystem.SelectUpgrade(upgrade);
                    Hide();
                }
            );
        }
    }

    // =========================
    // HIDE
    // =========================
    public void Hide()
    {
        panelRoot.SetActive(false);
        Time.timeScale = 1f;
    }
}
