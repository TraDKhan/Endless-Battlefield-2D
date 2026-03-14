using DG.Tweening;
using UnityEngine;

public class VFX_Warning_Line : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    [Header("Tween")]
    [SerializeField] float blinkDuration = 0.25f;

    Tween blinkTween;

    public void Init(float width, float duration)
    {
        transform.position = transform.position + Vector3.down * 0.2f;

        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(width - 1f, 0.3f);

        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        sr.DOFade(1f, 0.2f);

        blinkTween = sr.DOFade(0.2f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo);

        Destroy(gameObject, duration);
    }

    void OnDestroy()
    {
        blinkTween?.Kill();
    }
}