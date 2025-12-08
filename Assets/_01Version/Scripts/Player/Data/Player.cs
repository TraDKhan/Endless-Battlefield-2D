using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "Player/Player Data")]
public class Player : ScriptableObject
{
    public string playerName;
    public int baseHealth;
    public int baseEnergy;
    public int baseArmor;
    public int baseDamage;    
    public float baseMoveSpeed;
    public float baseAttackSpeed;
    public float baseCrit;
}
