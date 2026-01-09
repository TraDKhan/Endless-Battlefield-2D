#region Player
// ===== Player Stat
public enum PlayerStatType
{
    MaxHealth,
    Armor,
    MoveSpeed,
    Energy
}
// ===== Player Movement
public enum MoveDirection
{
    Up = 0,
    RightUp = 1,
    RightDown = 2,
    Down = 3,
    LeftDown = 4,
    LeftUp = 5
}
#endregion
// ===== Upgrade
#region Upgrade
public enum UpgradeTarget
{
    Player,
    Weapon
}

public enum UpgradeType
{
    PlayerStat,
    WeaponStat,
    Skill
}
#endregion
// ===== Item
#region Item
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
#endregion
// ===== Enemy
#region Enemy
//public enum EnemyType
//{
//    MeeleEnemy, 
//    RangeEnemy
//}
public enum EnemyAttackType
{
    Melee,
    Charge,
    Ranged
}

public enum EnemyStateType
{
    Idle,
    Chase,
    Attack,
    Reposition,
    Dead
}
#endregion

public enum ProjectileMode
{
    Direction,
    Position
}

// ===== WEAPON =====
public enum WeaponStatType
{
    Damage,
    Cooldown,
    CritChance,
    ProjectileCount,
    Range,
    ProjectileSpeed
}

public enum WeaponType
{
    Pistol,
    Shotgun,
    Laser,
    Boomerang
}