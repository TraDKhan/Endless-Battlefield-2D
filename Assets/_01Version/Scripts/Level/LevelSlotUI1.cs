using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSlotUI1 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameObjectLock;

    private LevelData level;
    private System.Action<LevelSlotUI1> onClicked;

    public LevelData Level => level;

    public void Bind(LevelData data, System.Action<LevelSlotUI1> clickCallback)
    {
        level = data;
        onClicked = clickCallback;

        levelText.text = $"{data.levelIndex}";

        bool unlocked = LevelProgress.IsUnlocked(data.levelIndex);
        gameObjectLock.gameObject.SetActive(!unlocked);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke(this);
    }
}
