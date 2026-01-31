using System;
using System.Collections.Generic;
using UnityEngine;
public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;

    [Header("Upgrade Pool")]
    public List<UpgradeData> upgradePool;

    [Header("Sub Systems")]
    [SerializeField] private WeaponUpgradeSystem weaponSystem;
    [SerializeField] private PlayerUpgradeSystem playerSystem;

    private int pendingLevelUps;
    private bool isChoosingUpgrade;

    private readonly List<BaseSkill> skills = new();

    private readonly HashSet<UnLockWeaponUpgrade> unlockedWeapons = new();

    public WeaponUpgradeSystem Weapon => weaponSystem;
    public PlayerUpgradeSystem Player => playerSystem;

    public event Action<List<UpgradeData>> OnShowUpgradeUI;
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
            playerLevelSystem.OnLevelUp += OnPlayerLevelUp;
    }

    private void Update()
    {
        TryShowUpgrade();
    }

    #region LEVEL FLOW
    // =========================
    // LEVEL FLOW
    // =========================
    private void OnPlayerLevelUp(int newLevel)
    {
        pendingLevelUps++;
    }

    private void TryShowUpgrade()
    {
        if (isChoosingUpgrade || pendingLevelUps <= 0)
            return;

        pendingLevelUps--;
        isChoosingUpgrade = true;

        Time.timeScale = 0f;
        OnShowUpgradeUI?.Invoke(GetRandomUpgrades(3));
    }

    public void SelectUpgrade(UpgradeData data)
    {
        if (!data.CanApply(this))
            return;

        data.Apply(this);

        isChoosingUpgrade = false;
        Time.timeScale = 1f;
    }
    #endregion

    #region SKILL
    // =========================
    // SKILL
    // =========================
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

        skill.Init(PlayerController.Instance.transform);

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
    // =========================
    // WEAPON
    // =========================
    public void UnlockWeapon(UnLockWeaponUpgrade data)
    {
        if (!unlockedWeapons.Add(data))
            return;

        var weaponGO = Instantiate(data.weaponPrefab);
        var controller = weaponGO.GetComponent<WeaponController>();
        var weaponData = controller.Data;

        Transform socket = WeaponSocketController.Instance.GetSocket(weaponData.slotType);

        weaponGO.transform.SetParent(socket);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;
    }

    public bool HasWeapon(UnLockWeaponUpgrade data)
    {
        return unlockedWeapons.Contains(data);
    }

    public void ApplyWeaponStatsUpgrade(WeaponUpgradeData data)
    {
        weaponSystem.ApplyUpgrade(data);
    }
    #endregion

    #region UTILS
    private List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> valid = new();
        foreach (var up in upgradePool)
            if (up.CanApply(this))
                valid.Add(up);

        List<UpgradeData> result = new();
        while (result.Count < count && valid.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, valid.Count);
            result.Add(valid[index]);
            valid.RemoveAt(index);
        }

        return result;
    }
    #endregion
}

