using UnityEngine;

public class CharacterStatSystem : StatSystem<CharacterStatType>
{
    protected override void Clamp()
    {
        if (finalStats.TryGetValue(CharacterStatType.MaxHP, out var hp))
            finalStats[CharacterStatType.MaxHP] = Mathf.Max(1, hp);

        if (finalStats.TryGetValue(CharacterStatType.MaxMP, out var mp))
            finalStats[CharacterStatType.MaxMP] = Mathf.Max(0, mp);

        if (finalStats.TryGetValue(CharacterStatType.MoveSpeed, out var speed))
            finalStats[CharacterStatType.MoveSpeed] = Mathf.Max(0, speed);
    }
}
