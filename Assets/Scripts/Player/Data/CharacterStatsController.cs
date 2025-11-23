using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    public Player playerData;
    public PlayerLevelSystem level;
    public PlayerEquipmentController equip;
    public PlayerBuffController buff;

    public CharacterStats Stats { get; private set; }

    void Awake()
    {
        Stats = new CharacterStats(playerData, level, equip, buff);
        Stats.RecalculateStats();
    }
}
