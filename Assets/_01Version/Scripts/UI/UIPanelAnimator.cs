using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelAnimator : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float showDuration = 0.25f;
    [SerializeField] private float hideDuration = 0.2f;

    private CanvasGroup canvasGroup;
    private RectTransform rect;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowAnim());
    }

    public void Hide()
    {
        if (!gameObject.activeInHierarchy) return;

        StopAllCoroutines();
        StartCoroutine(HideAnim());
    }

    IEnumerator ShowAnim()
    {
        float time = 0;

        rect.localScale = Vector3.one * 0.8f;
        canvasGroup.alpha = 0;

        while (time < showDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / showDuration;

            // Ease Out Back (bounce nhẹ)
            float scale = EaseOutBack(t);
            rect.localScale = Vector3.one * scale;

            canvasGroup.alpha = t;

            yield return null;
        }

        rect.localScale = Vector3.one;
        canvasGroup.alpha = 1;
    }

    IEnumerator HideAnim()
    {
        float time = 0;

        Vector3 startScale = rect.localScale;

        while (time < hideDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / hideDuration;

            rect.localScale = Vector3.Lerp(startScale, Vector3.one * 0.8f, t);
            canvasGroup.alpha = 1 - t;

            yield return null;
        }

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    // 🔥 Magic easing (rất quan trọng)
    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}