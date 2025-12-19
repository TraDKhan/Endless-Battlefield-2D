using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInputController inputController;
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CharacterStatsController statsController;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 1f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private int dashEnergyCost = 20;

    private bool isDashing;
    private float dashTimer;
    private Vector2 dashDirection;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float currentMoveSpeed;

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
    }

    private void OnStatsReady(CharacterStats stats)
    {
        stats.OnStatsChanged += OnStatsChanged;
        UpdateMoveSpeed();
    }

    private void Update()
    {
        movementInput = inputController.GetMovementVector();

        TryDash();

        HandleAnimation();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            DashMove();
        else
            Move();
    }
    // ===== Dash (Lướt)
    private void TryDash()
    {
        if (isDashing) return;
        if (!inputController.IsDashPressed()) return;
        if (movementInput.sqrMagnitude <= 0.01f) return;

        var energy = PlayerEnergyController.Instance;
        if (energy == null) return;

        if (!energy.Consume(dashEnergyCost)) return;

        isDashing = true;
        dashTimer = dashDuration;
        dashDirection = movementInput.normalized;

        // Optional: animation / effect
        //animationController?.PlayDash();
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
            isDashing = false;
    }

    // ===== Movement
    private void Move()
    {
        if (currentMoveSpeed <= 0f) return;

        Vector2 newPos = rb.position +
                         movementInput.normalized *
                         currentMoveSpeed *
                         Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }

    // ===== Stats
    private void OnStatsChanged()
    {
        UpdateMoveSpeed();
    }

    private void UpdateMoveSpeed()
    {
        currentMoveSpeed = statsController.Stats.GetMoveSpeed();
    }

    // ===== Visual
    private void HandleAnimation()
    {
        if (animationController == null) return;

        if (movementInput.sqrMagnitude > 0.01f)
            animationController.PlayWalk();
        else
            animationController.PlayIdle();
    }

    private void HandleFlip()
    {
        if (spriteRenderer == null) return;

        if (Mathf.Abs(movementInput.x) > 0.05f)
            spriteRenderer.flipX = movementInput.x < 0;
    }
}
