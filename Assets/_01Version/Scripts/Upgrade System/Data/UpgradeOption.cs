using System;

public enum UpgradeType
{
    Health,
    Damage,
    MoveSpeed,
    AttackSpeed,
    Crit,
    Range
}

[Serializable]
public class UpgradeOption
{
    public UpgradeType type;
    public float value;     // tăng bao nhiêu
    public string title;    // hiển thị UI
    public string desc;     // mô tả
}