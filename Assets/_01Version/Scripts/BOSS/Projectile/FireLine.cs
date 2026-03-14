using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FireLine : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] BoxCollider2D col;

    [SerializeField] int damage = 10;
    [SerializeField] float tickRate = 0.5f;

    float duration;
    float timer;

    public void Init(float width, float duration)
    {
        this.duration = duration;

        // set sprite width bằng draw mode sliced
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(width, sr.size.y);

        // set collider cùng kích thước
        col.offset = new Vector2(0.15f, -0.1f);
        col.size = new Vector2(width - 1f, col.size.y);

        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = tickRate;

            Debug.Log("Player nhận damage từ lửa 🔥");

            other.GetComponent<IDamageable>()?.TakeDamage(damage);
        }
    }
}