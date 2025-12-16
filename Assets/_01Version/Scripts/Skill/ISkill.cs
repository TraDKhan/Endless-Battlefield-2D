using UnityEngine;

public interface ISkill
{
    void Init(Transform owner, CharacterStats stats);
    void OnUnlock();
    void OnLevelUp();
}
