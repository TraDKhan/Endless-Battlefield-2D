using UnityEngine;

[RequireComponent (typeof(BossDecisionSystem))]
[RequireComponent (typeof(BossSkillExecutor))]
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
    [ContextMenu("Fire")]
    public void AnimationEvent(string eventId)
    {
        Debug.Log(eventId);
        executor.OnAnimationEvent(eventId);
    }
}
