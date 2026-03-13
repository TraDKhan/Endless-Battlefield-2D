using UnityEngine;

[RequireComponent(typeof(PoolIdentity))]
public class Boss_Projectile_Meteor : MonoBehaviour
{
    [SerializeField] float fallSpeed = 12f;
    [SerializeField] float cooldown = 0.5f;
    private float cooldownTimer = 0f;

    Vector3 targetPos;

    public void Init(Vector3 target)
    {
        targetPos = target;
        cooldownTimer = cooldown;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            fallSpeed * Time.deltaTime
        );

        if(transform.position == targetPos)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(2f, 2f, 1f), Time.deltaTime * 10f);
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            Impact();
        }
    }

    void Impact()
    {
        // gây damage / explosion

        //Destroy(gameObject);
    }
}