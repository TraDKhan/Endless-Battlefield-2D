using UnityEngine;

public class DropSystem : MonoBehaviour
{
    public DropTable dropTable;

    private EnemyController enemy;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
    }

    void OnEnable()
    {
        if (enemy != null)
            enemy.OnEnemyDead += HandleEnemyDead;
    }

    void OnDisable()
    {
        if (enemy != null)
            enemy.OnEnemyDead -= HandleEnemyDead;
    }

    private void HandleEnemyDead(EnemyController enemy)
    {
        DropItems(enemy.transform.position);
    }

    void DropItems(Vector3 pos)
    {
        if (dropTable == null) return;

        foreach (var drop in dropTable.drops)
        {
            if (Random.value > drop.dropChance)
                continue;

            int amount = drop.amount;

            for (int i = 0; i < amount; i++)
            {
                SpawnItem(drop.itemPrefab, pos);
            }
        }
    }

    void SpawnItem(GameObject prefab, Vector3 pos)
    {
        var item = ObjectPoolManager.Instance.Spawn<PooledItem>(prefab);

        if (item == null) return;

        Vector2 offset = Random.insideUnitCircle * 0.6f;

        item.transform.position = pos + (Vector3)offset;
    }
}
