using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PoolIdentity))]
public class VFX_Warning_Circle : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    public PoolIdentity Identity { get; set; }

    [Header("Timing")]
    [SerializeField] private float totalDuration = 0.8f;
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.2f;

    [Header("Scale")]
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField]
    private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Fade")]
    [SerializeField] private float maxAlpha = 0.8f;

    private SpriteRenderer sr;
    private Sequence sequence;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        CreateTween();
    }

    private void Start()
    {
        OnSpawn();
        Play(transform.position);
    }

    void CreateTween()
    {
        float middleTime = totalDuration - fadeInTime - fadeOutTime;

        sequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Pause();

        sequence.Join(
            transform.DOScale(endScale, totalDuration)
            .SetEase(scaleCurve)
        );

        sequence.Join(
            sr.DOFade(maxAlpha, fadeInTime)
        );

        sequence.Insert(
            totalDuration - fadeOutTime,
            sr.DOFade(0f, fadeOutTime)
        );

        sequence.OnComplete(() =>
        {
            ObjectPoolManager.Instance.Despawn(this);
        });
    }

    #region Pool

    public void OnSpawn()
    {
        transform.localScale = startScale;
        SetAlpha(0f);

        sequence.Rewind();
    }

    public void OnDespawn()
    {
        sequence.Pause();
    }

    #endregion

    public void Play(Vector2 position, System.Action onComplete = null)
    {
        transform.position = position;

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            ObjectPoolManager.Instance.Despawn(this);
        });

        sequence.Restart();
    }

    void SetAlpha(float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
