using UnityEngine;

public class CharacterStats
{
    private readonly StatSystem statSystem;

    public CharacterStats(StatSystem system)
    {
        statSystem = system;
    }

    public int MaxHP => Mathf.RoundToInt(
        statSystem.GetStat(StatContext.Character, StatType.MaxHP));

    public int MaxMP => Mathf.RoundToInt(
        statSystem.GetStat(StatContext.Character, StatType.MaxMP));

    public float Armor =>
        statSystem.GetStat(StatContext.Character, StatType.Armor);

    public float MoveSpeed =>
        statSystem.GetStat(StatContext.Character, StatType.MoveSpeed);
}
