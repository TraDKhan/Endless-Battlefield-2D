using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public LineRenderer line;
    public int segments = 12;         // số điểm zigzag
    public float jaggedness = 0.2f;   // độ zigzag
    public float duration = 0.15f;    // tồn tại bao lâu

    private float timer;

    public void Init(Vector3 start, Vector3 end)
    {
        if (line == null) line = GetComponent<LineRenderer>();

        line.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);

            // vị trí đường thẳng
            Vector3 pos = Vector3.Lerp(start, end, t);

            // thêm zigzag
            pos.x += Random.Range(-jaggedness, jaggedness);
            pos.y += Random.Range(-jaggedness, jaggedness);

            line.SetPosition(i, pos);
        }

        timer = duration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
