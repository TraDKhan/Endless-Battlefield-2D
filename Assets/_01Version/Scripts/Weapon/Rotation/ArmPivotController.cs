using UnityEngine;

public class ArmPivotController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer characterRenderer;

    [Header("Rotation")]
    [SerializeField] private float rotationOffset = 0f;
    [SerializeField] private bool flipCharacter = true;

    public void AimAt(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += rotationOffset;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (flipCharacter && characterRenderer != null)
        {
            characterRenderer.flipX = direction.x < 0;
        }
    }
}
