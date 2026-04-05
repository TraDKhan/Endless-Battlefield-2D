using System.Runtime.CompilerServices;
using UnityEngine;
using DamageNumbersPro;

public class PopupController : MonoBehaviour
{
    [Header("Popup Prefabs")]
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject healPopupPrefab;

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

        if (damagePopupPrefab == null)
        {
            Debug.LogError("Damage Popup Prefab is NULL");
            return;
        }
    }
    private Vector3 GetRandomOffset()
    {
        return new Vector3(
            UnityEngine.Random.Range(-0.25f, 0.25f),
            UnityEngine.Random.Range(0f, 0.25f),
            0f
        );
    }

    //public void ShowDamage(int amount, Vector3 worldPos, bool isCrit = false)
    //{
    //    //var popup = ObjectPoolManager.Instance
    //    //    .Spawn<TextPopup>(damagePopupPrefab);

    //    //if (popup == null) return;

    //    //popup.transform.position = worldPos + GetRandomOffset();
    //    //popup.Play(amount, isCrit ? Color.red : Color.white);
    //    DamageNumber damageNumber = damageNumberPrefab.Spawn()
    //}

    public void ShowDamage(Vector3 position, int number) => damageNumberPrefab.Spawn(position, number); 
    public void ShowCirtDamage(Vector3 position, int number) => damageCritPrefab.Spawn(position, number);
    public void ShowExp(Vector3 position, int number) => expNumberPrefab.Spawn(position, number);
    public void ShowCoin(Vector3 position, int number) => coinNumberPrefab.Spawn(position, number);
    public void ShowHeal(Vector3 position, int number) => healNumberPrefab.Spawn(position, number);
}
