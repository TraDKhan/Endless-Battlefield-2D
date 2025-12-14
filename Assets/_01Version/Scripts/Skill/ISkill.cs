public interface ISkill
{
    void Init(CharacterStats ownerStats);
    void OnUnlock();
    void OnLevelUp();
}
