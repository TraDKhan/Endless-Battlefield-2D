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

        bool found = false;

        foreach (var level in levels)
        {
            bool active = level.levelIndex == selected;
            level.gameObject.SetActive(active);

            if (active) found = true;
        }

        if (!found)
            Debug.LogError("Level not found: " + selected);
    }
}
