using System.Collections;
using UnityEngine;

public interface IBossSkill
{
    string SkillID { get; }

    bool CanExecute(BossContext ctx);
    IEnumerator Execute(BossContext ctx);
}

