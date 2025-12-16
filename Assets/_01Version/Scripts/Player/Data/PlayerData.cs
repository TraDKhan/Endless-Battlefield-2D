using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Info")]
    public string playerName;

    [Header("Base Stats")]
    public int baseHealth;
    public int baseEnergy;
    public int baseArmor;
    public float baseMoveSpeed;

    [Header("Weapon")]
    public WeaponType startWeapon;
}
