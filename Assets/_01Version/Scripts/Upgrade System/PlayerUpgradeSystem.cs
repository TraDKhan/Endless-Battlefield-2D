using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeSystem : MonoBehaviour, IStatSource<CharacterStatType>
{
    private readonly Dictionary<CharacterStatType, StatUpgrade> upgrades = new();

    public void Apply(PlayerUpgradeData data)
    {
        if (!upgrades.TryGetValue(data.statType, out var up))
        {
            up = new StatUpgrade
            {
                level = 0,
                valuePerLevel = data.valuePerLevel
            };
        }

        up.level++;
        upgrades[data.statType] = up;
        PlayerController.Instance.RecalculateStats();
    }

    public IEnumerable<StatModifier<CharacterStatType>> GetModifiers()
    {
        foreach (var kv in upgrades)
        {
            Debug.Log($"Upgrade {kv.Key} = {kv.Value.Value}");
            yield return new StatModifier<CharacterStatType>
            {
                statType = kv.Key,
                value = kv.Value.Value
            };
        }
    }

    public int GetLevel(CharacterStatType stat)
        => upgrades.TryGetValue(stat, out var up) ? up.level : 0;
}
