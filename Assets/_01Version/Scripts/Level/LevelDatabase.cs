using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public LevelData[] levels;

    public LevelData GetLevel(int index)
    {
        return levels[index - 1];
    }

    public int MaxLevel => levels.Length;
}