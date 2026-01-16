using System.Collections;

public interface IBasicAttack
{
    bool CanAttack(BossContext ctx);
    IEnumerator Attack(BossContext ctx);
}
