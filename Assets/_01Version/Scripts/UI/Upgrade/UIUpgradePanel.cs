using System.Collections.Generic;
using UnityEngine;

public class UIUpgradePanel : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Upgrade option slots (3 item UI)")]
    public UIUpgradeOption[] optionSlots;

    private List<UpgradeData> currentOptions;

    void Start()
    {
        if (PlayerUpgradeSystem.Instance != null)
            PlayerUpgradeSystem.Instance.OnShowUpgradeUI += Show;

        Hide();
    }

    // show 3 UpgradeData option
    void Show(List<UpgradeData> options)
    {
        currentOptions = options;
        panelRoot.SetActive(true);

        for (int i = 0; i < optionSlots.Length; i++)
        {
            int index = i;
            var upgrade = options[i];

            // Cập nhật UI
            optionSlots[i].SetData(upgrade);

            // Gán hành động khi click
            optionSlots[i].button.onClick.RemoveAllListeners();
            optionSlots[i].button.onClick.AddListener(() =>
            {
                PlayerUpgradeSystem.Instance.ApplyUpgrade(currentOptions[index]);
                Hide();
            });
        }

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        panelRoot.SetActive(false);
        Time.timeScale = 1f;
    }
}
