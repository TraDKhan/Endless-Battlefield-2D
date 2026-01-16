#region PLAYER
// ===== Player Stat ===== \\
public enum PlayerStatType
{
    MaxHealth,
    Armor,
    MoveSpeed,
    Energy
}
// ===== Player Movement ====== \\
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

#region UPGRADE
// ===== type ====== \\
public enum UpgradeType
{
    PlayerStat,
    WeaponStat,
    Skill,
    NewWeapon
}
#endregion

#region ITEM
// ===== Item ===== \\
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
#endregion

#region ENEMY
// ===== attack ===== \\
public enum EnemyAttackType
{
    Melee,
    Charge,
    Ranged
}

// ===== state ===== \\
public enum EnemyStateType
{
    Idle,
    Chase,
    Attack,
    Reposition,
    Dead
}
public enum EnemyStateID
{
    Idle,
    Chase,
    Attack,
    Dead
}
// ===== mode ===== \\
public enum ProjectileMode
{
    Direction,
    Position
}
#endregion

#region WEAPON
// ===== stats ===== \\
public enum WeaponStatType
{
    Damage,
    Cooldown,
    CritChance,
    ProjectileCount,
    Range,
    ProjectileSpeed
}

// ===== type ===== \\
public enum WeaponType
{
    Pistol,
    Shotgun,
    Laser,
    Boomerang
}

// ===== socket ===== \\
public enum WeaponSlotType
{
    MainHand,
    OffHand
}
#endregion

public enum ProjectileMoveType
{
    Straight,
    Homing
}

// ===== BOSS
public enum BossSkillType
{
    Basic,
    Special
}