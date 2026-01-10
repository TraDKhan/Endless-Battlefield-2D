using System.Runtime.CompilerServices;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [Header("Popup Prefabs")]
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject healPopupPrefab;

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

    public void ShowDamage(int amount, Vector3 worldPos, bool isCrit = false)
    {
        var popup = ObjectPoolManager.Instance
            .Spawn<DamagePopup>(damagePopupPrefab);

        if (popup == null) return;

        popup.transform.position = worldPos + GetRandomOffset();
        popup.Play(amount, isCrit ? Color.red : Color.white);
    }

    public void ShowHeal(int amount, Vector3 worldPos)
    {
        var popup = ObjectPoolManager.Instance
            .Spawn<DamagePopup>(healPopupPrefab);

        popup.transform.position = worldPos;
        popup.Play(amount, Color.green);
    }
}
