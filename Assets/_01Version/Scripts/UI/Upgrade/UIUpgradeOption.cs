
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgradeOption : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text description;
    public Button button;

    public void SetData(UpgradeData data)
    {
        //if (icon != null)
        //    icon.sprite = data.icon;

        if (title != null)
            title.text = data.upgradeName;

        if (description != null)
            description.text = data.description;
    }
}
