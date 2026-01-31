public class WeaponStatSystem : StatSystem<WeaponStatType>
{
    protected override void Clamp()
    {
        // Cooldown không được < 0.05s
        if (GetStat(WeaponStatType.Cooldown) < 0.05f)
            finalStats[WeaponStatType.Cooldown] = 0.05f;

        // ProjectileCount >= 1
        if (GetStat(WeaponStatType.ProjectileCount) < 1)
            finalStats[WeaponStatType.ProjectileCount] = 1;
    }
}
