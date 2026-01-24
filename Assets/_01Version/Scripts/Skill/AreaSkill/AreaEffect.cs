using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AreaEffect : MonoBehaviour
{
    private AreaEffectDamage damageLogic;

    private void Awake()
    {
        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;

        damageLogic = GetComponent<AreaEffectDamage>();
    }

    public void Init(int damage, float tickInterval)
    {
        damageLogic.Init(damage, tickInterval);
    }

    public void SetScale(float diameter)
    {
        transform.localScale = Vector3.one * diameter;
    }
}
