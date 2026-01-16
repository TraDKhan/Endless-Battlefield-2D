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

    State currentState;

    BossDecisionSystem decision;
    BossSkillExecutor executor;
    IBasicAttack basicAttack;

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
    }

    // ================= STATES =================

    void UpdateIdle()
    {
        if (executor.IsBusy)
            return;

        thinkTimer -= Time.deltaTime;
        if (thinkTimer > 0f)
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

        //ƯU TIÊN SKILL
        IBossSkill skill = decision.ChooseSkill(executor.Context);
        if (skill != null)
        {
            executor.ExecuteSkill(skill);
            currentState = State.UsingSkill;
            return;
        }

        //KHÔNG CÓ SKILL → ĐÁNH THƯỜNG
        if (basicAttack != null && basicAttack.CanAttack(executor.Context))
        {
            executor.ExecuteBasicAttack(basicAttack);
            currentState = State.BasicAttack;
            return;
        }

        currentState = State.Idle;
    }

    void UpdateUsingSkill()
    {
        if (!executor.IsBusy)
        {
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
