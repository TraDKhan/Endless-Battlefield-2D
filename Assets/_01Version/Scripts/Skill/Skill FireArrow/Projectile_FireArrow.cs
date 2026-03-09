using UnityEngine;

public class Projectile_FireArrow : MonoBehaviour
{
    private Transform owner;
    private float damage;
    private float speed;

    private float angle;

    private bool isOrbit = true;
    private Vector2 moveDir;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Int(Transform _owner, float _damage, float _speed)
    {
        owner = _owner;
        damage = _damage;
        speed = _speed;
    }

    public void SetAngle(float _angle)
    {
        angle = _angle;
    }

    public void Launch(Vector2 dir)
    {
        isOrbit = false;
        moveDir = dir;

        // Trigger animation Fly
        if (animator != null)
            animator.SetTrigger("Launch");
    }

    private void Update()
    {
        if (isOrbit)
            Orbit();
        else
            Fly();
    }

    void Orbit()
    {
        if (owner == null) return;

        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);

        transform.position = owner.position + offset;
        // Mũi tên hướng về player
        Vector2 dirToPlayer = (transform.position - owner.position).normalized;
        transform.right = dirToPlayer;
    }

    void Fly()
    {
        transform.position += (Vector3)moveDir * speed * 5f * Time.deltaTime;

        // xoay mũi tên theo hướng bay
        transform.right = moveDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        if (collision.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage((int)damage);
        }

        Destroy(gameObject);
    }
}