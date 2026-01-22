using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetail : MonoBehaviour
{
    public static UIItemDetail Instance { get; private set; }

    [Header("UI")]
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text statsText;

    void Awake()
    {
        Instance = this;
    }

    public void Show(InventoryItem item)
    {
        icon.sprite = item.data.icon;
        nameText.text = item.data.itemName;
        descriptionText.text = item.data.description;

        statsText.text = BuildStatText(item.data.stats);
        Debug.Log(statsText.text);
    }

    string BuildStatText(List<StatEntry> stats)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var stat in stats)
        {
            sb.AppendLine($"{stat.statType}: +{stat.value}");
        }

        return sb.ToString();
    }
}
