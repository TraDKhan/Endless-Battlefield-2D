using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Data")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;

    public List<StatModifier> baseStats;
}
