using System.Collections.Generic;
using System.Linq;

public class EquippedItemStatSource : IStatSource
{
    private readonly List<StatModifier> mods;

    public EquippedItemStatSource(ItemData data)
    {
        mods = data.stats.Select(s => new StatModifier
        {
            statType = s.statType,
            value = s.value,
            modType = s.modType,
            context = s.context
        }).ToList();
    }

    public IEnumerable<StatModifier> GetModifiers() => mods;
}