public class PlayerLevelSystem
{
    private int currentLevel;
    private int currentEXP;

    public int GetHealthBonus() => currentLevel * 10;
    public int GetDamageBonus() => currentLevel * 2;
}
