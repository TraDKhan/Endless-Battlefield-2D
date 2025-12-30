using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleLayer;

    public bool IsPlayerInRange(float range)
    {
        if (!player) return false;

        return Vector2.Distance(
            transform.position,
            player.position
        ) <= range;
    }

}
