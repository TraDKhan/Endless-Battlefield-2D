using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgradeOption : MonoBehaviour
{
    public Image icon;       // Image minh họa
    public Button button;    // Nút bấm
    public TMP_Text label;   // Title + mô tả

    // Gắn dữ liệu vào UI Item
    public void SetData(UpgradeOption option)
    {
        // Gắn text
        label.text = $"{option.title}\n<size=70%>{option.desc}</size>";

        // Nếu sau này có icon thì gán vào:
        // icon.sprite = option.icon;
    }
}
