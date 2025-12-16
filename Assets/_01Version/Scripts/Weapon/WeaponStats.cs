[System.Serializable]
public class WeaponStats
{
    public float damage;
    public float cooldown;
    public float range;
    public float projectileSpeed;
    public int projectileCount;
    public float critChance;

    public void ApplyBonus(WeaponStats bonus)
    {
        damage += bonus.damage;
        cooldown += bonus.cooldown;
        range += bonus.range;
        projectileSpeed += bonus.projectileSpeed;
        projectileCount += bonus.projectileCount;
        critChance += bonus.critChance;
    }

}
