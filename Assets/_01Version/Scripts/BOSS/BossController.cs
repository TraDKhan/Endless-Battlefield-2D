using UnityEngine;

public class BossController : MonoBehaviour
{
    public EnemyStats stats;

    [Header("Components")]
    public BossPhaseController phaseController;
    public EnemyMovement movement;
    public EnemyAnimationController anim;

    [Header("Runtime")]
    public Transform player;
    public LayerMask targetLayer;
    public int CurrentPhase => phaseController.CurrentPhase;

    void Awake()
    {
        phaseController = GetComponent<BossPhaseController>();
        movement = GetComponent<EnemyMovement>();
        anim = GetComponent<EnemyAnimationController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {

    }

    void Update()
    {
        if (!player) return;

        HandleMovement();
    }

    void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Nếu quá gần thì dừng (chuẩn bị attack)
        if (distance <= stats.personalSpace)
        {
            movement.Stop();
            anim.SetMoving(false);
            return;
        }

        float speed = stats.moveSpeed;

        switch (CurrentPhase)
        {
            case 1:
                speed *= 0.8f;
                break;

            case 2:
                speed *= 1.5f;
                break;
        }

        movement.MoveTowards(player.position, speed);
        anim.SetMoving(true);
    }

    private void OnDrawGizmos()
    {
        if (stats == null) return;

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stats.personalSpace);
    }
}
