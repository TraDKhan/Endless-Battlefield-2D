using UnityEngine;

public class LaserWeapon : Weapon
{
    public LineRenderer line;

    public override void Shoot(Vector3 direction)
    {
        base.Shoot(direction);

        // Raycast theo range
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            currentStats.range
        );

        Vector3 endPos = hit ? hit.point : transform.position + direction * currentStats.range;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, endPos);
    }
}
