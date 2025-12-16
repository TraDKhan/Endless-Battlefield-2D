using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Base Stats")]
    public int baseDamage;
    public float baseCooldown;
    public float baseCritChance;
    public int baseProjectileCount;
    public float baseRange;
    public float baseProjectileSpeed;

    [Header("Projectile")]
    public GameObject projectilePrefab;
}
