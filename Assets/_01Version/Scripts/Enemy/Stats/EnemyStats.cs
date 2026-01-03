using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Enemy Data")]
public class EnemyStats : ScriptableObject
{
    public EnemyAttackType enemyType;

    [Header("Base stats")]
    public int maxHealth = 100;
    public int damage = 1;
    public float moveSpeed = 2.5f;
    public float detectRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;

    [Header("Meele Enemy")]
    public float meleeStopRange = 0.4f;
    public float meleeTolerance = 0.1f;

    [Header("Range Enemy")]
    public float preferredRange = 4f;
    public float rangeTolerance = 0.5f;
}