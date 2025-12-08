using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgradePanel : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Upgrade option slots (3 item UI)")]
    public UIUpgradeOption[] optionSlots;

    private List<UpgradeOption> currentOptions;

    void Start()
    {
        if (PlayerUpgradeSystem.Instance != null)
            PlayerUpgradeSystem.Instance.OnShowUpgradeUI += Show;

        Hide();
    }

    // Hiện UI mỗi khi lên level
    void Show(List<UpgradeOption> options)
    {
        currentOptions = options;
        panelRoot.SetActive(true);

        for (int i = 0; i < optionSlots.Length; i++)
        {
            int index = i;
            optionSlots[i].SetData(options[i]);

            optionSlots[i].button.onClick.RemoveAllListeners();
            optionSlots[i].button.onClick.AddListener(() =>
            {
                PlayerUpgradeSystem.Instance.ApplyUpgrade(currentOptions[index]);
                Hide();
            });
        }

        Time.timeScale = 0f; // Pause game khi chọn upgrade
    }

    public void Hide()
    {
        panelRoot.SetActive(false);
        Time.timeScale = 1f;
    }
}
