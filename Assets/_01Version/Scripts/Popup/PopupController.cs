using System.Runtime.CompilerServices;
using UnityEngine;
using DamageNumbersPro;

public class PopupController : MonoBehaviour
{
    [Header("Popup Prefabs")]
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private DamageNumber damageCritPrefab;
    [SerializeField] private DamageNumber healNumberPrefab;
    [SerializeField] private DamageNumber expNumberPrefab;
    [SerializeField] private DamageNumber coinNumberPrefab;

    public static PopupController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowDamage(Vector3 position, int number) => damageNumberPrefab.Spawn(position, number); 
    public void ShowCirtDamage(Vector3 position, int number) => damageCritPrefab.Spawn(position, number);
    public void ShowExp(Vector3 position, int number) => expNumberPrefab.Spawn(position, number);
    public void ShowCoin(Vector3 position, int number) => coinNumberPrefab.Spawn(position, number);
    public void ShowHeal(Vector3 position, int number) => healNumberPrefab.Spawn(position, number);
}
