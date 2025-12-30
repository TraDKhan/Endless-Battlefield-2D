using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public EnemyType enemyType;

    public float moveSpeed = 2.5f;
    public float detectRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;
    public int damage = 1;

    [Header("Meele Enemy")]
    public float meleeStopRange = 0.4f;
    public float meleeTolerance = 0.1f;

    [Header("Range Enemy")]
    public float preferredRange = 4f;
    public float rangeTolerance = 0.5f;
}