using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameObjectLock;
    [SerializeField] private Image iconImage;

    private LevelData level;
    private System.Action<LevelSlotUI> onClicked;

    public LevelData Level => level;

    public void Bind(LevelData data, System.Action<LevelSlotUI> clickCallback)
    {
        level = data;
        onClicked = clickCallback;

        levelText.text = $"Level {data.levelIndex}";
        iconImage.sprite = data.icon;

        bool unlocked = LevelProgress.IsUnlocked(data.levelIndex);
        gameObjectLock.gameObject.SetActive(!unlocked);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (!LevelProgress.IsUnlocked(level.levelIndex))
        //    return;

        onClicked?.Invoke(this);
    }
}
