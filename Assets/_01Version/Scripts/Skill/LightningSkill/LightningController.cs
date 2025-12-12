using UnityEngine;
public class LightningController : MonoBehaviour
{
    [SerializeField] private GameObject _lightningSkillPrefab; // prefab gốc
    private GameObject _lightningSkillInstance;                // instance trong scene

    public Transform playerTransform;

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                Debug.LogError("Không tìm thấy Player trong scene!");
        }

        CastLightningSkill(); // tạo skill lần đầu
    }

    void CastLightningSkill()
    {
        if (_lightningSkillInstance != null)
        {
            Destroy(_lightningSkillInstance); // xoá instance cũ nếu còn
        }
        _lightningSkillInstance = Instantiate(_lightningSkillPrefab, playerTransform.position, Quaternion.identity);

        // Gắn skill làm con của player
        _lightningSkillInstance.transform.SetParent(playerTransform);

        _lightningSkillInstance.SetActive(true);
    }
}

