using System;

#region PLAYER

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

public enum CharacterStatType 
{
    MaxHP,
    Armor,
    MoveSpeed
}

[Serializable]
public class CStatEntry
{
    public CharacterStatType statType;
    public float value;
}

public enum WeaponStatType
{
    Damage,
    CritChance,
    Cooldown,
    AttackRange,
    ProjectileCount,
    ProjectileSpeed
}

[Serializable]
public class WStatEntry
{
    public WeaponStatType statType;
    public float value;
}

public enum SkillStatType
{
    Damage,
    CritChance,
    Cooldown,
    AttackRange,
    ProjectileCount,
    ProjectileSpeed,
    Duration,
    LightningCount,
    RotateSpeed
}

[Serializable]
public class SKStatEntry
{
    public SkillStatType statType;
    public float value;
}

public enum ItemType
{
    Equipment,
    Consumable,
    Material
}

public enum EquipmentSlotType
{
    Helmet,
    Armor,
    Weapon,
    Ring,
    Amulet
}
