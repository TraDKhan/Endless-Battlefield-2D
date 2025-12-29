using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInputController inputController;
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private CharacterStatsController statsController;

    [Header("Movement")]
    [SerializeField] private float deadZone = 0.01f;
    [SerializeField] private float directionThreshold = 0.35f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 1f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private int dashEnergyCost = 20;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float currentMoveSpeed;

    // Direction
    private MoveDirection currentDirection = MoveDirection.Down;
    private MoveDirection lastDirection = MoveDirection.Down;

    // Dash
    private bool isDashing;
    private float dashTimer;
    private Vector2 dashDirection;
    private Vector2 lastMoveVector = Vector2.down;

    #region Unity

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (statsController == null)
            statsController = CharacterStatsController.Instance;
    }

    private void OnEnable()
    {
        CharacterStatsController.OnStatsReady += OnStatsReady;
    }

    private void OnDisable()
    {
        CharacterStatsController.OnStatsReady -= OnStatsReady;
        if (statsController?.Stats != null)
            statsController.Stats.OnStatsChanged -= OnStatsChanged;
    }

    private void Update()
    {
        movementInput = inputController.GetMovementVector();

        UpdateDirection(movementInput);
        TryDash();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            DashMove();
        else
            Move();
    }
    #endregion

    #region Movement

    private void Move()
    {
        if (currentMoveSpeed <= 0f) return;
        if (movementInput.sqrMagnitude < deadZone * deadZone) return;

        Vector2 newPos = rb.position +
                         movementInput.normalized *
                         currentMoveSpeed *
                         Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }

    #endregion

    #region Direction (6 hướng SUVI)

    private void UpdateDirection(Vector2 input)
    {
        if (input.sqrMagnitude < deadZone * deadZone)
            return;

        float x = input.x;
        float y = input.y;

        // ===== NỬA TRÊN =====
        if (y > directionThreshold)
        {
            if (x > directionThreshold)
                currentDirection = MoveDirection.RightUp;
            else if (x < -directionThreshold)
                currentDirection = MoveDirection.LeftUp;
            else
                currentDirection = MoveDirection.Up;
        }
        // ===== NỬA DƯỚI =====
        else if (y < -directionThreshold)
        {
            if (x > directionThreshold)
                currentDirection = MoveDirection.RightDown;
            else if (x < -directionThreshold)
                currentDirection = MoveDirection.LeftDown;
            else
                currentDirection = MoveDirection.Down;
        }
        else
        {
            // Gần trục ngang → giữ hướng cũ (tránh rung)
            return;
        }

        lastMoveVector = DirectionToVector(currentDirection);
        lastDirection = currentDirection;
    }

    #endregion

    #region Dash

    private void TryDash()
    {
        if (isDashing) return;
        if (!inputController.IsDashPressed()) return;
        if (movementInput.sqrMagnitude < deadZone * deadZone) return;

        var energy = PlayerEnergyController.Instance;
        if (energy == null) return;
        if (!energy.Consume(dashEnergyCost)) return;

        isDashing = true;
        dashTimer = dashDuration;
        dashDirection = DirectionToVector(currentDirection);
        animationController.SetDash(true);
    }

    private void DashMove()
    {
        dashTimer -= Time.fixedDeltaTime;

        float dashSpeed = dashDistance / dashDuration;

        Vector2 newPos = rb.position +
                         dashDirection *
                         dashSpeed *
                         Time.fixedDeltaTime;

        rb.MovePosition(newPos);

        if (dashTimer <= 0f)
        {
            isDashing = false;
            animationController.SetDash(false);
        }
    }

    #endregion

    #region Direction Helper

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

    #region Stats

    private void OnStatsReady(CharacterStats stats)
    {
        stats.OnStatsChanged += OnStatsChanged;
        UpdateMoveSpeed();
    }

    private void OnStatsChanged()
    {
        UpdateMoveSpeed();
    }

    private void UpdateMoveSpeed()
    {
        currentMoveSpeed = statsController.Stats.GetMoveSpeed();
    }

    #endregion
}
