using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInputController inputController;
    [SerializeField] private PlayerAnimationController animationController;

    [Header("Movement")]
    [SerializeField] private float deadZone = 0.01f;
    [SerializeField] private float directionThreshold = 0.35f;

    private Rigidbody2D rb;
    private CharacterStatSystem stats;

    private Vector2 movementInput;

    private MoveDirection currentDirection = MoveDirection.Down;
    private Vector2 lastMoveVector = Vector2.down;

    #region Init

    public void Initialize(PlayerController controller)
    {
        stats = controller.StatSystem;

        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    #endregion

    private void Update()
    {
        movementInput = inputController.GetMovementVector();

        UpdateDirection(movementInput);
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Movement

    private void Move()
    {
        float moveSpeed = stats.GetStat(CharacterStatType.MoveSpeed);
        if (moveSpeed <= 0f) return;
        if (movementInput.sqrMagnitude < deadZone * deadZone) return;

        rb.MovePosition(
            rb.position +
            movementInput.normalized *
            moveSpeed *
            Time.fixedDeltaTime
        );
    }

    #endregion

    #region Direction Helper

    private void UpdateDirection(Vector2 input)
    {
        if (input.sqrMagnitude < deadZone * deadZone)
            return;

        float x = input.x;
        float y = input.y;

        // 1️⃣ Ưu tiên UP khi nhấn rõ
        if (y > directionThreshold)
        {
            if (x > directionThreshold)
                currentDirection = MoveDirection.RightUp;
            else if (x < -directionThreshold)
                currentDirection = MoveDirection.LeftUp;
            else
                currentDirection = MoveDirection.Up;
        }
        // 2️⃣ DOWN khi nhấn xuống
        else if (y < -directionThreshold)
        {
            if (x > directionThreshold)
                currentDirection = MoveDirection.RightDown;
            else if (x < -directionThreshold)
                currentDirection = MoveDirection.LeftDown;
            else
                currentDirection = MoveDirection.Down;
        }
        // 3️⃣ KHÔNG có Y rõ ràng → ép về DOWN
        else
        {
            if (x > directionThreshold)
                currentDirection = MoveDirection.RightDown;
            else if (x < -directionThreshold)
                currentDirection = MoveDirection.LeftDown;
            else
                currentDirection = MoveDirection.Down;
        }

        lastMoveVector = DirectionToVector(currentDirection);
    }


    private Vector2 DirectionToVector(MoveDirection dir)
    {
        return dir switch
        {
            MoveDirection.Up => Vector2.up,
            MoveDirection.Down => Vector2.down,
            MoveDirection.LeftUp => new Vector2(-1f, 1f).normalized,
            MoveDirection.RightUp => new Vector2(1f, 1f).normalized,
            MoveDirection.LeftDown => new Vector2(-1f, -1f).normalized,
            MoveDirection.RightDown => new Vector2(1f, -1f).normalized,
            _ => Vector2.down
        };
    }

    #endregion

    #region Animation

    private void UpdateAnimation()
    {
        if (animationController == null) return;

        bool isMoving = movementInput.sqrMagnitude >= deadZone * deadZone;

        animationController.SetMovement(
            isMoving,
            lastMoveVector
        );
    }

    #endregion
}
