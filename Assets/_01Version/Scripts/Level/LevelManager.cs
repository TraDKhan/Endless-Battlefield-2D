using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelRoot[] levels;

    void Awake()
    {
        levels = GetComponentsInChildren<LevelRoot>(true);
    }

    void Start()
    {
        ActivateSelectedLevel();
    }

    void ActivateSelectedLevel()
    {
        int selected = SelectedLevelRuntime.SelectedLevelIndex;

        foreach (var level in levels)
        {
            level.gameObject.SetActive(level.levelIndex == selected);
        }
    }
}
