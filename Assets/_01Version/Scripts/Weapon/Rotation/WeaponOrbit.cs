using UnityEngine;

public class WeaponOrbit : MonoBehaviour
{
    [Header("Orbit")]
    [SerializeField] private float radius = 1.2f;
    [SerializeField] private float stepAngle = 30f; // độ / lần bắn

    [Header("Visual")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float rotationOffset;

    private Transform owner;
    private float angle;

    void Awake()
    {
        owner = transform.parent;
    }

    void Start()
    {
        UpdateOrbit();
    }

    // =========================
    public void StepOrbit()
    {
        angle += stepAngle;
        UpdateOrbit();
    }

    void UpdateOrbit()
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(
            Mathf.Cos(rad),
            Mathf.Sin(rad)
        ) * radius;

        transform.position = (Vector2)owner.position + offset;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        Vector2 dir = FireDirection();
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + rotationOffset);

        if (sprite != null)
            sprite.flipY = dir.x < 0;
    }

    // =========================
    public Vector2 FireDirection()
    {
        return (transform.position - owner.position).normalized;
    }
}
