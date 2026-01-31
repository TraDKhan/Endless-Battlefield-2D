[System.Serializable]
public struct StatUpgrade
{
    public int level;
    public float valuePerLevel;

    public float Value => level * valuePerLevel;
}