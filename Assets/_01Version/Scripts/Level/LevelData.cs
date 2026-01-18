using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public Sprite icon;
    public bool isUnlocked;
    public string gameplaySceneName;
}
