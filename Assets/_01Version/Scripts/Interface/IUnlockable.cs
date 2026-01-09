using UnityEngine;

public interface IUnlockable
{
    void Init(Transform owner, CharacterStats stats);
    void OnUnlock();
    void OnLevelUp();
    int Level { get; }
}
