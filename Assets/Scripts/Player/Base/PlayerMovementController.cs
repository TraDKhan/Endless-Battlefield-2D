using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInputController inputController;
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private SpriteRenderer spriteRenderer;   // Để flip trái/phải

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movementInput = inputController.GetMovementVector();

        // Animation
        if (animationController != null)
        {
            if (movementInput.magnitude > 0.01f)
                animationController.PlayWalk();
            else
                animationController.PlayIdle();
        }

        // Flip hướng nhân vật nếu đang di chuyển trái/phải
        if (spriteRenderer != null && Mathf.Abs(movementInput.x) > 0.05f)
        {
            spriteRenderer.flipX = movementInput.x < 0;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 newPos = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
