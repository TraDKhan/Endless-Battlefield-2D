using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PoolIdentity))]
public class DamagePopup : MonoBehaviour, IPoolable
{
    [Header("Motion")]
    public float moveUpDistance = 0.8f;
    public float duration = 1f;

    [Header("Scale")]
    public Vector3 startScale = Vector3.zero;
    public Vector3 peakScale = Vector3.one * 1.2f;
    public Vector3 endScale = Vector3.one * 0.8f;

    private TextMeshPro text;
    private Sequence sequence;

    public PoolIdentity Identity { get; set; }

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    // ================= POOL =================
    public void OnSpawn()
    {
        gameObject.SetActive(true);

        // Reset
        transform.localScale = startScale;
        text.alpha = 1f;

        sequence?.Kill();
        transform.DOKill();
    }

    public void OnDespawn()
    {
        sequence?.Kill();
        gameObject.SetActive(false);
    }

    // ================= PLAY =================
    public void Play(int damage, Color color)
    {
        text.text = damage.ToString();
        text.color = color;

        Vector3 startPos = transform.position;

        sequence = DOTween.Sequence();

        // Move up
        sequence.Join(
            transform.DOMoveY(startPos.y + moveUpDistance, duration)
                     .SetEase(Ease.OutQuad)
        );

        // Scale up
        sequence.Join(
            transform.DOScale(peakScale, duration * 0.3f)
                     .SetEase(Ease.OutBack)
        );

        // Scale down
        sequence.Append(
            transform.DOScale(endScale, duration * 0.7f)
                     .SetEase(Ease.InQuad)
        );

        // Fade
        sequence.Join(
            text.DOFade(0f, duration * 0.4f)
                .SetDelay(duration * 0.6f)
        );

        // 👉 QUAN TRỌNG: trả về pool
        sequence.OnComplete(() =>
        {
            ObjectPoolManager.Instance.Despawn(this);
        });
    }
}
