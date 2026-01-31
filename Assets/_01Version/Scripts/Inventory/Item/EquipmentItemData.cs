using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentItemData : ItemData
{
    public EquipmentSlotType slot;

    public List<StatModifier<CharacterStatType>> playerStats;
    public List<StatModifier<WeaponStatType>> weaponStats;

    public AnimationCurve upgradeScaling;
}
