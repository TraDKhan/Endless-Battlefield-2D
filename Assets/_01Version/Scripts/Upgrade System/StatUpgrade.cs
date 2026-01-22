[System.Serializable]
public struct StatUpgrade
{
    public int level;
    public float valuePerLevel;
    public StatModType modType;

    public float Value => level * valuePerLevel;
}