using UnityEngine;

[CreateAssetMenu(menuName = "Endless/Enemy Director")]
public class EnemyDirector : ScriptableObject
{
    [Header("Enemy Pools")]
    public GameObject[] earlyEnemies;
    public GameObject[] midEnemies;
    public GameObject[] lateEnemies;
    public GameObject[] eliteEnemies;

    [Header("Boss")]
    public BossController[] bosses;

    public GameObject GetEnemy(float time, float difficulty)
    {
        if (time < 60f)
            return RandomFrom(earlyEnemies);

        if (time < 180f)
            return RandomFrom(midEnemies);

        if (time < 300f)
            return RandomFrom(lateEnemies);

        // late game mix
        if (Random.value < 0.2f)
            return RandomFrom(eliteEnemies);

        return RandomFrom(lateEnemies);
    }

    public BossController GetBoss(float time)
    {
        int index = Mathf.Min((int)(time / 60f), bosses.Length - 1);
        return bosses[index];
    }

    GameObject RandomFrom(GameObject[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }
}