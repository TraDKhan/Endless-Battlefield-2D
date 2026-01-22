using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private DropTable dropTable;

    private EnemyController enemy;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();

        if (enemy == null)
            Debug.Log("[Drop Item] EnemyController is not found!");
    }

    void OnEnable()
    {
        enemy.OnEnemyDead += DropItems;
    }

    void OnDisable()
    {
        enemy.OnEnemyDead -= DropItems;
    }

    public void DropItems(EnemyController enemy)
    {
        if (dropTable == null) return;

        foreach (var entry in dropTable.drops)
        {
            float roll = Random.Range(0f, 100f);
            if (roll > entry.dropChance) continue;

            for (int i = 0; i < entry.amount; i++)
            {
                SpawnItem(entry.item);
            }
        }
    }

    private void SpawnItem(ItemData_tamthoi item)
    {
        if (item == null || item.prefab == null) return;

        Vector2 offset = Random.insideUnitCircle * 0.5f;

        var pooledItem = ObjectPoolManager.Instance.Spawn<PooledItem>(item.prefab);

        pooledItem.transform.position = (Vector2)transform.position + offset;
    }

    // Optional: roll 1 item theo weight
    public ItemData_tamthoi RollOneItem()
    {
        if (dropTable == null) return null;

        float totalWeight = 0;
        foreach (var d in dropTable.drops)
            totalWeight += d.dropChance;

        float roll = Random.Range(0, totalWeight);
        float current = 0;

        foreach (var d in dropTable.drops)
        {
            current += d.dropChance;
            if (roll <= current)
                return d.item;
        }

        return null;
    }
}
