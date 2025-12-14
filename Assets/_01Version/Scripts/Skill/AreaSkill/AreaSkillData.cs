using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/AreaSkillData")]
public class AreaSkillData : ScriptableObject
{
    public float radius = 3f;
    public float tickInterval = 0.3f;   // mỗi 0.3s gây damage
    public int damage = 10;
    public float duration = 5f;         // tồn tại 5s
    public float cooldown = 2f;
}

