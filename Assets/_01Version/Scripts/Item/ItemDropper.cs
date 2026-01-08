using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private DropTable dropTable;
    void Awake()
    {
        var enemyBase = GetComponent<EnemyBase>();
        if (enemyBase != null)
            enemyBase.OnDeath += DropItems;
    }

    void OnDestroy()
    {
        var enemyBase = GetComponent<EnemyBase>();
        if (enemyBase != null)
            enemyBase.OnDeath -= DropItems;
    }


    [ContextMenu ("Drop Item")]
    public void DropItems()
    {
        if (dropTable == null) return;
        Debug.Log("Spawn Item");

        foreach (var entry in dropTable.drops)
        {
            float roll = Random.Range(0f, 100f);
            if (roll > entry.dropChance) continue;

            int amount = entry.amount;
            for (int i = 0; i < amount; i++)
            {
                SpawnItem(entry.item);
            }
        }
    }

    private void SpawnItem(ItemData item)
    {
        if (item == null || item.prefab == null) return;
        Debug.Log("Drop Item");

        Vector2 offset = Random.insideUnitCircle * 0.5f;
        Instantiate(item.prefab, (Vector2)transform.position + offset, Quaternion.identity);
    }
    public ItemData RollOneItem()
    {
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
