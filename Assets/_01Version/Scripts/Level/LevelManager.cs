using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelDatabase database;

    private const string LEVEL_REACHED_KEY = "LevelReached";
    private const string SELECTED_LEVEL_KEY = "SelectedLevel";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetCurrentLevel()
    {
        int level = PlayerPrefs.GetInt(LEVEL_REACHED_KEY, 3);
        return Mathf.Clamp(level, 1, GetMaxLevel());
    }
    public int GetSelectedLevel()
    {
        int level = PlayerPrefs.GetInt(SELECTED_LEVEL_KEY, 1);
        return Mathf.Clamp(level, 1, GetMaxLevel());
    }

    public int GetMaxLevel()
    {
        return database.MaxLevel;
    }

    public bool IsUnlocked(int level)
    {
        return level <= GetCurrentLevel();
    }

    public void LoadLevel(int level)
    {
        if (!IsUnlocked(level))
        {
            Debug.LogWarning($"Level {level} chưa mở khóa!");
            return;
        }

        // Lưu level được chọn
        PlayerPrefs.SetInt(SELECTED_LEVEL_KEY, level);
        PlayerPrefs.Save();

        // Load scene gameplay
        SceneManager.LoadScene("GameScene");
    }

    public void UnlockNextLevel(int level)
    {
        int current = GetCurrentLevel();

        if (level >= current && level < database.MaxLevel)
        {
            PlayerPrefs.SetInt(LEVEL_REACHED_KEY, level + 1);
            PlayerPrefs.Save();
        }
    }

    public LevelData GetLevelData(int level)
    {
        return database.GetLevel(level);
    }
}