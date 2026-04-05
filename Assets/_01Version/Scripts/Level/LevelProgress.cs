using UnityEngine;

public static class LevelProgress
{
    private const string KEY = "LEVEL_UNLOCKED";

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(KEY, 0);
    }

    public static void UnlockNextLevel(int currentLevel)
    {
        int unlocked = GetUnlockedLevel();

        if (currentLevel >= unlocked)
        {
            PlayerPrefs.SetInt(KEY, currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public static bool IsUnlocked(int levelIndex)
    {
        return levelIndex <= GetUnlockedLevel();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(KEY);
    }
}