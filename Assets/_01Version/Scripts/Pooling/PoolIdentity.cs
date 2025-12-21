using UnityEngine;

public class PoolIdentity : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (prefab == null)
            prefab = gameObject; // prefab gốc
    }
#endif
}
