using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentItemData : ItemData, IStatSource<CharacterStatType>,
    IStatSource<WeaponStatType>
{
    public EquipmentSlotType slot;

    public List<StatModifier<CharacterStatType>> playerStats;
    public List<StatModifier<WeaponStatType>> weaponStats;

    public IEnumerable<StatModifier<CharacterStatType>> GetModifiers()
        => playerStats;

    IEnumerable<StatModifier<WeaponStatType>>
        IStatSource<WeaponStatType>.GetModifiers()
        => weaponStats;
}
