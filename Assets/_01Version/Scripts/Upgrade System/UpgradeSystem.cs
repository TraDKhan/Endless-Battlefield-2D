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

    //sửa lại không cho WeaponSystem và PlayerSystem là public để có thể truy cập từ UpgradeData,
    // truyển init cho các logic cấp thấp hơn như SkillUpgradeData, WeaponUpgradeData, PlayerUpgradeData
    // để tránh việc các UpgradeData có thể truy cập trực tiếp vào WeaponSystem và Player
    // khởi tạo trong class cấp thấp và gắn nó thông qua init 
    public WeaponUpgradeSystem WeaponSystem => weaponSystem;
    public PlayerUpgradeSystem PlayerSystem => playerSystem;

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
        var playerLevelSystem = PlayerController.Instance.LevelSystem;
        if (playerLevelSystem != null)
            playerLevelSystem.OnLevelUp += OnPlayerLevelUp;

        //đăng ký cập nhật stats
        var player = PlayerController.Instance;
        if (player != null)
            player.StatSystem.AddSource(playerSystem);
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
        AudioManager.Instance.PlayLevelUp();
        pendingLevelUps--;
        isChoosingUpgrade = true;

        Time.timeScale = 0f;
        Debug.Log(OnShowUpgradeUI?.GetInvocationList().Length);
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

