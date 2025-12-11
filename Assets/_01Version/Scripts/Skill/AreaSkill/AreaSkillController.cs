using UnityEngine;

public class AreaSkillController : MonoBehaviour
{
    [SerializeField] private GameObject _areaSkill;
    public Transform playerTransform;
    public float duration = 5f;         // tồn tại 5s
    public float cooldown = 2f;
    private float cooldownTimer;
    private float lifeTimer;

    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerTransform = player.transform;
        }
        CastDamageArea();

        lifeTimer = duration;
        cooldownTimer = cooldown;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            _areaSkill.SetActive(false);
            cooldownTimer -= Time.deltaTime;
            Debug.Log("Skill Area dang hoi");
            if (cooldownTimer <= 0f)
            {
                _areaSkill.SetActive(true);
                lifeTimer = duration;
                cooldownTimer = cooldown;
                Debug.Log("Skil Area dang dc trien khai");
            }
        }
    }
    void CastDamageArea()
    {
        Instantiate(_areaSkill, transform.position, Quaternion.identity);
    }
}

