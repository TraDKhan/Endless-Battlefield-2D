using UnityEngine;

[RequireComponent(typeof(BossDecisionSystem))]
[RequireComponent(typeof(BossSkillExecutor))]
public class BossBrain : MonoBehaviour
{
    enum State
    {
        Idle,
        Decide,
        UsingSkill,
        BasicAttack
    }

    [SerializeField] float thinkInterval = 0.3f;
    [SerializeField] float postSkillRecovery = 1.0f;
    [SerializeField] int maxBasicAttackBetweenSkills = 2;

    State currentState;

    BossDecisionSystem decision;
    BossSkillExecutor executor;
    IBasicAttack basicAttack;

    float recoveryTimer;
    int basicAttackCount;
    float thinkTimer;

    void Awake()
    {
        decision = GetComponent<BossDecisionSystem>();
        executor = GetComponent<BossSkillExecutor>();
        basicAttack = GetComponent<IBasicAttack>();

        currentState = State.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;

            case State.Decide:
                UpdateDecide();
                break;

            case State.UsingSkill:
                UpdateUsingSkill();
                break;

            case State.BasicAttack:
                UpdateBasicAttack();
                break;
        }
        Debug.Log(currentState);
    }

    // ================= STATES =================

    void UpdateIdle()
    {
        if (executor.IsBusy)
            return;

        if (recoveryTimer > 0)
        {
            recoveryTimer -= Time.deltaTime;
            return;
        }

        thinkTimer -= Time.deltaTime;
        if (thinkTimer > 0)
            return;

        thinkTimer = thinkInterval;
        currentState = State.Decide;
    }

    void UpdateDecide()
    {
        if (executor.IsBusy)
        {
            currentState = State.Idle;
            return;
        }

        IBossSkill skill = decision.ChooseSkill(executor.Context);
        if (skill != null)
        {
            executor.ExecuteSkill(skill);
            currentState = State.UsingSkill;
            return;
        }

        if (basicAttackCount < maxBasicAttackBetweenSkills &&
            basicAttack != null &&
            basicAttack.CanAttack(executor.Context))
        {
            executor.ExecuteBasicAttack(basicAttack);
            basicAttackCount++;
            currentState = State.BasicAttack;
            return;
        }

        currentState = State.Idle;
    }

    void UpdateUsingSkill()
    {
        if (!executor.IsBusy)
        {
            recoveryTimer = postSkillRecovery;
            basicAttackCount = 0;
            currentState = State.Idle;
        }
    }

    void UpdateBasicAttack()
    {
        if (!executor.IsBusy)
        {
            currentState = State.Idle;
        }
    }
}
