[System.Serializable]
public class StatUpgradeProgress
{
    public int Level { get; private set; }
    private float valuePerLevel;

    public StatUpgradeProgress(float valuePerLevel)
    {
        this.valuePerLevel = valuePerLevel;
        Level = 0;
    }

    public void LevelUp()
    {
        Level++;
    }

    public float GetValue()
    {
        return Level * valuePerLevel;
    }
}
