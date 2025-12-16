using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData data;
    protected WeaponStats currentStats;

    protected float lastShootTime;

    protected virtual void Start()
    {
        currentStats = new WeaponStats();
        currentStats.ApplyBonus(data.baseStats);
    }

    public virtual bool CanShoot()
    {
        return Time.time - lastShootTime >= currentStats.cooldown;
    }

    public virtual void Shoot(Vector3 direction)
    {
        if (!CanShoot()) return;

        lastShootTime = Time.time;
    }

    public void ApplyUpgrade(WeaponStats bonus)
    {
        currentStats.ApplyBonus(bonus);
    }
    public struct DamageContext
    {
        public float damage;
        public float critChance;
        public float critMultiplier;
        public GameObject source; // weapon / player
    }

    protected DamageContext CreateDamageContext()
    {
        return new DamageContext
        {
            damage = currentStats.damage,
            critChance = currentStats.critChance,
            critMultiplier = 2f,
            source = gameObject
        };
    }

}
