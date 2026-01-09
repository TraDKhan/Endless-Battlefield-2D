using System;
using System.Collections.Generic;
using UnityEngine;
public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;

    [Header("Upgrade Pool")]
    public List<UpgradeData> upgradePool;

    public event Action<List<UpgradeData>> OnShowUpgradeUI;

    private int pendingLevelUps = 0;
    private bool isChoosingUpgrade = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        var playerLevelSystem = FindFirstObjectByType<PlayerLevelSystem>();
        if (playerLevelSystem != null)
        {
            playerLevelSystem.OnLevelUp += OnPlayerLevelUp;
        }
    }
    private void Update()
    {
        ShowUpgradeOptions();
    }

    #region CORE
    public void OnPlayerLevelUp(int newLevel)
    {
        pendingLevelUps++;
    }

    private void ShowUpgradeOptions()
    {
        if (isChoosingUpgrade) return;
        if (pendingLevelUps <= 0) return;

        isChoosingUpgrade = true;
        pendingLevelUps--;

        Time.timeScale = 0f;
        List<UpgradeData> options = RandomUpgrades(3);
        OnShowUpgradeUI?.Invoke(options);
    }

    List<UpgradeData> RandomUpgrades(int count)
    {
        if (upgradePool == null || upgradePool.Count == 0)
        {
            Debug.LogWarning("Upgrade pool is empty or null!");
            return new List<UpgradeData>();
        }

        List<UpgradeData> validPool = new List<UpgradeData>();

        foreach (var up in upgradePool)
        {
            if (up.CanApply())
                validPool.Add(up);
        }

        List<UpgradeData> result = new List<UpgradeData>();

        while (result.Count < count && validPool.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, validPool.Count);
            result.Add(validPool[index]);
            validPool.RemoveAt(index);
        }

        return result;
    }

    public void SelectUpgrade(UpgradeData data)
    {
        data.Apply();

        isChoosingUpgrade = false;
        Time.timeScale = 1f;
    }
    #endregion

    #region PLAYER
    // ===== PLAYER STAT ===== \\
    Dictionary<PlayerStatType, int> statLevels = new();
    public void IncreasePlayerStatLevel(PlayerStatType stat, float value)
    {
        if (!statLevels.ContainsKey(stat))
            statLevels[stat] = 0;

        statLevels[stat]++;

        CharacterStatsController.Instance.Stats.RecalculateStats();
    }
    public float GetPlayerStatBonus(PlayerStatType stat)
    {
        int level = GetPlayerStatLevel(stat);

        foreach (var up in upgradePool)
        {
            if (up is PlayerStatUpgradeData statUp && statUp.statType == stat)
            {
                return statUp.value * level;
            }
        }
        return 0f;
    }
    public int GetPlayerStatLevel(PlayerStatType stat) => statLevels.ContainsKey(stat) ? statLevels[stat] : 0;
    #endregion

    #region SKILL
    // ===== SKILL ===== \\
    List<BaseSkill> skills = new();
    public void ApplySkillUpgrade(SkillUpgradeData data)
    {
        foreach (var s in skills)
        {
            if (s.UpgradeData == data)
            {
                s.OnLevelUp();
                return;
            }
        }

        var go = Instantiate(data.skillPrefab);
        var skill = go.GetComponent<BaseSkill>();

        skill.Init(
            CharacterStatsController.Instance.transform,
            CharacterStatsController.Instance.Stats
        );

        skill.OnUnlock();
        skills.Add(skill);
    }

    public int GetSkillLevel(SkillUpgradeData data)
    {
        foreach (var s in skills)
            if (s.UpgradeData == data)
                return s.Level;

        return 0;
    }
    #endregion

    #region WEAPON
    // ===== WEAPON ===== \\
    HashSet<WeaponUpgradeData> unlockedWeapons = new();

    //public void UnlockWeapon(WeaponUpgradeData data)
    //{
    //    if (unlockedWeapons.Contains(data))
    //        return;

    //    unlockedWeapons.Add(data);

    //    var weapon = Instantiate(data.weaponPrefab);
    //    weapon.transform.SetParent(CharacterStatsController.Instance.transform);
    //}
    public void UnlockWeapon(WeaponUpgradeData data)
    {
        if (unlockedWeapons.Contains(data))
            return;

        unlockedWeapons.Add(data);

        var weaponGO = Instantiate(data.weaponPrefab);

        var weapon = weaponGO.GetComponent<Weapon>();
        var weaponData = weapon.data;

        Transform socket = WeaponSocketController.Instance.GetSocket(weaponData.slotType);

        weaponGO.transform.SetParent(socket);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;
    }

    public bool HasWeapon(WeaponUpgradeData data)
    {
        return unlockedWeapons.Contains(data);
    }
    #endregion

}

