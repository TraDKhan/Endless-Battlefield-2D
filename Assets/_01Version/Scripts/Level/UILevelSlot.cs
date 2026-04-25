using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _gameObjectLock;
    [SerializeField] private Button _btn;

    private int _index;

    public void Setup(int level)
    {
        _index = level;
        var data = LevelManager.Instance.GetLevelData(level);
        bool isUnlocked = LevelManager.Instance.IsUnlocked(level);

        _levelText.text = level.ToString();
        _gameObjectLock.SetActive(!isUnlocked);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool isUnlocked = LevelManager.Instance.IsUnlocked(_index);

        if (!isUnlocked)
        {
            Debug.Log("Level chưa mở khóa!");
            return;
        }

        //todo: map với loadLevel
        LevelManager.Instance.LoadLevel(_index);
    }
}
