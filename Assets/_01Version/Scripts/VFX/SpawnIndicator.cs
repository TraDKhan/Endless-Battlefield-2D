//using System.Collections;
//using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
//public class SpawnIndicator : MonoBehaviour, IPoolable
//{
//    [Header("Pool")]
//    public PoolIdentity Identity { get; set; }

//    [Header("Timing")]
//    [SerializeField] private float totalDuration = 0.8f;
//    [SerializeField] private float fadeInTime = 0.2f;
//    [SerializeField] private float fadeOutTime = 0.2f;

//    [Header("Scale")]
//    [SerializeField] private Vector3 startScale = Vector3.zero;
//    [SerializeField] private Vector3 endScale = Vector3.one;
//    [SerializeField]
//    private AnimationCurve scaleCurve =
//        AnimationCurve.EaseInOut(0, 0, 1, 1);

//    [Header("Fade")]
//    [SerializeField] private float maxAlpha = 0.8f;

//    private SpriteRenderer sr;
//    private Coroutine playRoutine;

//    void Awake()
//    {
//        sr = GetComponent<SpriteRenderer>();
//    }

//    #region Pool Callbacks
//    public void OnSpawn()
//    {
//        //if (Identity == null || Identity.gameObject == null)
//        //{
//        //    Debug.LogError("[SpawnIndicator] PoolIdentity chưa được gán!");
//        //    return;
//        //}

//        //if (Identity.gameObject.name.Contains("(Clone)"))
//        //{
//        //    Debug.LogError(
//        //        "[SpawnIndicator] PoolIdentity đang trỏ vào CLONE – gán sai prefab!"
//        //    );
//        //}

//        SetAlpha(0f);
//        transform.localScale = startScale;
//    }


//    public void OnDespawn()
//    {
//        if (playRoutine != null)
//        {
//            StopCoroutine(playRoutine);
//            playRoutine = null;
//        }
//    }
//    #endregion

//    public void Play(Vector2 position, System.Action onComplete)
//    {
//        transform.position = position;

//        playRoutine = StartCoroutine(PlayRoutine(onComplete));
//    }

//    IEnumerator PlayRoutine(System.Action onComplete)
//    {
//        float timer = 0f;

//        while (timer < totalDuration)
//        {
//            timer += Time.deltaTime;
//            float t = Mathf.Clamp01(timer / totalDuration);

//            // Scale
//            float scaleT = scaleCurve.Evaluate(t);
//            transform.localScale = Vector3.Lerp(startScale, endScale, scaleT);

//            // Alpha
//            float alpha;
//            if (timer < fadeInTime)
//            {
//                alpha = Mathf.Lerp(0f, maxAlpha, timer / fadeInTime);
//            }
//            else if (timer > totalDuration - fadeOutTime)
//            {
//                alpha = Mathf.Lerp(
//                    maxAlpha,
//                    0f,
//                    (timer - (totalDuration - fadeOutTime)) / fadeOutTime
//                );
//            }
//            else
//            {
//                alpha = maxAlpha;
//            }

//            SetAlpha(alpha);
//            yield return null;
//        }

//        SetAlpha(0f);
//        onComplete?.Invoke();

//        // 🔥 QUAN TRỌNG: trả về pool
//        ObjectPoolManager.Instance.Despawn(this);
//    }

//    void SetAlpha(float alpha)
//    {
//        Color c = sr.color;
//        c.a = alpha;
//        sr.color = c;
//    }
//}
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class SpawnIndicator : MonoBehaviour, IPoolable
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