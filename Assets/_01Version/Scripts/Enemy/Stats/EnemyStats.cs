using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Enemy Stat")]
public class EnemyStats : ScriptableObject
{
    public EnemyAttackType enemyType;

    [Header("Base")]
    public int maxHealth = 100;
    public float moveSpeed = 2.5f;

    [Header("Attack")]
    public int damage = 1;
    public float attackCooldown = 1f;

    [Header("Khoảng cách")]
    public float attackRange = 1.2f; //tấn công
    public float personalSpace = 0.6f; // khoảng cách với player
}