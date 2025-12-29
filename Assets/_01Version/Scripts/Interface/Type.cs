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