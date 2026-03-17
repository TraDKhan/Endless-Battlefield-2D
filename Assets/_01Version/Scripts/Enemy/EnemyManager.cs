using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public event Action<int> OnEnemyDeath;
    private int killCount;

    private void Awake()
    {
        Instance = this;
    }

    public void NotifyEnemyDeath()
    {
        killCount++;
        OnEnemyDeath?.Invoke(killCount);
        Debug.Log($"Enemy killed. Total: {killCount}");
    }

    public int KillCount => killCount;
}