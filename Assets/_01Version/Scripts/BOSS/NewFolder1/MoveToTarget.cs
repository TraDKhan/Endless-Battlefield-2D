using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    private Vector3 target;
    private float speed;

    public void Init(Vector3 targetPos, float moveSpeed)
    {
        target = targetPos;
        speed = moveSpeed;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }
}