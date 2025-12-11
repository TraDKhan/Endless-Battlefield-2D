using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningController : MonoBehaviour
{
    public LightningSkill data;
    private float timer;
    public GameObject lightningEffectPrefab;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            CastLightning();
            timer = data.Cooldown;
        }
    }

    void CastLightning()
    {
        // Tìm enemy trong vùng
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, data.Radius, LayerMask.GetMask("Enemy"));

        if (enemies.Length == 0) return;

        for (int i = 0; i < data.StrikesPerCast; i++)
        {
            // Lấy enemy random
            var target = enemies[Random.Range(0, enemies.Length)];

            // Tạo hiệu ứng sét
            SpawnLightningEffect(target.transform.position);
            Debug.Log("Set: tan cpng" + target.name);

            // Gây damage
            target.GetComponent<EnemyHealthController>().TakeDamage(data.Damage);
        }
    }

    void SpawnLightningEffect(Vector3 targetPos)
    {
        // vị trí bắt đầu tia (trên trời)
        Vector3 start = targetPos + new Vector3(0, 4, 0);

        GameObject obj = Instantiate(lightningEffectPrefab);
        obj.GetComponent<LightningEffect>().Init(start, targetPos);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, data.Radius);
    }
}

