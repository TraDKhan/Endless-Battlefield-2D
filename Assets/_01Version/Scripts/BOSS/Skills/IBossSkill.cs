using System.Collections;

public interface IBossSkill
{
    string SkillID { get; }
    bool CanExecute(BossContext ctx);
    IEnumerator Execute(BossContext ctx);
    float Cooldown { get; }
}

