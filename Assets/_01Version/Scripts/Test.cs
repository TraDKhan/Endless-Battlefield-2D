using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        var playerLevelSystem = new PlayerLevelSystem();
        var playerUpgradeSystem = new PlayerUpgradeSystem();

        // giả lập kết nối event
        playerLevelSystem.OnLevelUp += playerUpgradeSystem.HandleLevelUp;
        playerUpgradeSystem.OnShowUpgradeUI += (options) =>
        {
            Debug.Log($"UI hiển thị {options.Count} upgrade");
        };

        // ===== TEST CASE =====
        Debug.Log("=== TEST: AddEXP đủ lên 3 level ===");

        // Level 1 → cần 50 exp
        // Level 2 → cần 75 exp
        // Level 3 → cần 100 exp
        // Tổng để lên từ lv1 → lv4 = 50 + 75 + 100 = 225 exp

        playerLevelSystem.AddEXP(225);

        Debug.Log($"Kết quả: Level hiện tại = {playerLevelSystem.CurrentLevel}, EXP còn lại = {playerLevelSystem.CurrentEXP}");
    }
}