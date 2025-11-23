public class PlayerBuffController
{
    private int buffHealth;
    private int buffDamage;
    private float buffMoveSpeed;

    public void AddBuff(int hp, int dmg, float spd)
    {
        buffHealth += hp;
        buffDamage += dmg;
        buffMoveSpeed += spd;
    }

    public int GetBuffHealthBonus() => buffHealth;
    public int GetBuffDamageBonus() => buffDamage;
    public float GetBuffMoveSpeedBonus() => buffMoveSpeed;
}
