using System;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentEXP { get; private set; } = 0;

    public int ExpToNextLevel => 50 + (CurrentLevel - 1) * 25;

    // ====== BONUS mỗi lần lên cấp ======
    [Header("Level Up Bonus")]
    public int BonusHealthPerLevel => 50 + (CurrentLevel - 1) * 20;
    public int BonusDamagePerLevel => 10 + (CurrentLevel - 1) * 3;

    // Sự kiện
    public event Action<int, int, int> OnExpChanged;      // currentExp, nextExp, level
    public event Action<int> OnLevelUp;                   // newLevel
    public event Action<int, int> OnStatsBonusApplied;    // hpBonus, damageBonus

    //gọi hàm hồi HP khi lên cấp

    [ContextMenu("+ EXP")]
    public void AddEXP1()
    {

        CurrentEXP += 100;

        // Có thể lên nhiều level 1 lúc
        while (CurrentEXP >= ExpToNextLevel)
        {
            CurrentEXP -= ExpToNextLevel;
            LevelUp();
        }

        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);
    }
    public void AddEXP(int value)
    {
        if (value <= 0) return;

        CurrentEXP += value;

        // Có thể lên nhiều level 1 lúc
        while (CurrentEXP >= ExpToNextLevel)
        {
            CurrentEXP -= ExpToNextLevel;
            LevelUp();
        }

        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);
    }

    [ContextMenu("UP LEVEL")]
    private void LevelUp()
    {
        CurrentLevel++;

        // Tính bonus theo Level
        int hpBonus = BonusHealthPerLevel;
        int damageBonus = BonusDamagePerLevel;

        // Gửi sự kiện cộng chỉ số
        OnStatsBonusApplied?.Invoke(hpBonus, damageBonus);

        // Báo UI
        OnLevelUp?.Invoke(CurrentLevel);
        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);

        Debug.Log("Up");
    }

    // ====== API để CharacterStats lấy bonus ======
    public int GetHealthBonus() => BonusHealthPerLevel;
    public int GetDamageBonus() => BonusDamagePerLevel;
}
