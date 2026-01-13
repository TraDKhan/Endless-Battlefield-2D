using UnityEngine;

public class PoolIdentity : MonoBehaviour
{
    [SerializeField] private string poolKey;
    public string PoolKey => poolKey;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(poolKey))
            poolKey = gameObject.name;
    }
#endif
}