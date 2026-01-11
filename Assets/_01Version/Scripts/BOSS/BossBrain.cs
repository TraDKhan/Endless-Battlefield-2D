using UnityEngine;

public class BossBrain : MonoBehaviour
{
    private BossDecisionSystem decision;
    private BossSkillExecutor executor;

    [SerializeField] private float thinkInterval = 0.3f;
    private float timer;

    void Awake()
    {
        decision = GetComponent<BossDecisionSystem>();
        executor = GetComponent<BossSkillExecutor>();
    }

    void Update()
    {
        if (executor.IsBusy)
            return;

        timer -= Time.deltaTime;
        if (timer > 0f)
            return;

        timer = thinkInterval;

        IBossSkill skill = decision.ChooseSkill(executor.Context);
        if (skill != null)
        {
            executor.ExecuteSkill(skill);
        }
    }
}
