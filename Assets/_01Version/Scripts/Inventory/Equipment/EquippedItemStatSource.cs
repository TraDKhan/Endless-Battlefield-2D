using System.Collections.Generic;

public class EquippedItemStatSource : IStatSource
{
    private ItemData data;
    private List<StatModifier> modifiers = new();

    public ItemData Data => data;

    public EquippedItemStatSource(ItemData data)
    {
        if (data.itemType != ItemType.Equipment)
            throw new System.ArgumentException(
                $"Item {data.name} is not Equipment"
            );

        this.data = data;

        foreach (var s in data.stats)
        {
            modifiers.Add(new StatModifier
            {
                statType = s.statType,
                value = s.value,
                modType = s.modType
            });
        }
    }

    public List<StatModifier> GetModifiers()
    {
        return modifiers;
    }
}
