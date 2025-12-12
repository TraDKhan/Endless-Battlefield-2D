using UnityEngine;

public class AreaSkillController : MonoBehaviour
{
    [SerializeField] private GameObject _areaSkillPrefab; // prefab gốc
    private GameObject _areaSkillInstance;                // instance trong scene

    public Transform playerTransform;
    public float duration = 5f;   // tồn tại 5s
    public float cooldown = 2f;   // hồi chiêu 2s

    private float cooldownTimer;
    private float lifeTimer;

    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                Debug.LogError("Không tìm thấy Player trong scene!");
        }

        CastDamageArea(); // tạo skill lần đầu
        lifeTimer = duration;
        cooldownTimer = cooldown;
    }

    void Update()
    {
        if (_areaSkillInstance != null && _areaSkillInstance.activeSelf)
        {
            // skill đang hoạt động
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                _areaSkillInstance.SetActive(false);
                cooldownTimer = cooldown;
                Debug.Log("Skill Area đang hồi");
            }
        }
        else
        {
            // skill đang hồi
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                CastDamageArea();
                lifeTimer = duration;
                Debug.Log("Skill Area được triển khai lại");
            }
        }
    }

    void CastDamageArea()
    {
        if (_areaSkillInstance != null)
        {
            Destroy(_areaSkillInstance); // xoá instance cũ nếu còn
        }
        _areaSkillInstance = Instantiate(_areaSkillPrefab, playerTransform.position, Quaternion.identity);

        // Gắn skill làm con của player
        _areaSkillInstance.transform.SetParent(playerTransform);

        _areaSkillInstance.SetActive(true);
    }
}