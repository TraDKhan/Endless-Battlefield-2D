using UnityEngine;

public interface ISkill
{
    void Init(Transform owner);
    void OnUnlock();
    void OnLevelUp();
}
