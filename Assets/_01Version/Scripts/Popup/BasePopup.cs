using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public abstract class BasePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected CanvasGroup background;
    [SerializeField] protected RectTransform panel;
    [SerializeField] protected CanvasGroup panelCanvasGroup;

    [Header("Animation Settings")]
    [SerializeField] protected float fadeDuration = 0.25f;
    [SerializeField] protected float scaleDuration = 0.3f;
    [SerializeField] protected Ease showEase = Ease.OutBack;
    [SerializeField] protected Ease hideEase = Ease.InBack;

    protected Sequence currentSequence;

    protected virtual void Awake()
    {
        InitState();
    }

    protected virtual void InitState()
    {
        background.alpha = 0;
        panel.localScale = Vector3.zero;
        panelCanvasGroup.alpha = 0;
    }

    #region PUBLIC API

    public virtual void Show()
    {
        gameObject.SetActive(true);

        currentSequence?.Kill();
        currentSequence = DOTween.Sequence();

        currentSequence.Append(background.DOFade(1f, fadeDuration));

        currentSequence.Join(panel.DOScale(Vector3.one, scaleDuration)
            .SetEase(showEase));

        currentSequence.Join(panelCanvasGroup.DOFade(1f, fadeDuration));

        currentSequence.OnComplete(OnShowComplete);
    }

    public virtual void Hide()
    {
        currentSequence?.Kill();
        currentSequence = DOTween.Sequence();

        currentSequence.Append(background.DOFade(0f, fadeDuration));

        currentSequence.Join(panel.DOScale(Vector3.zero, scaleDuration)
            .SetEase(hideEase));

        currentSequence.Join(panelCanvasGroup.DOFade(0f, fadeDuration));

        currentSequence.OnComplete(() =>
        {
            OnHideComplete();
            gameObject.SetActive(false);
        });
    }

    #endregion

    #region CALLBACKS (OVERRIDE)

    protected virtual void OnShowComplete() { }

    protected virtual void OnHideComplete() { }

    #endregion

    #region OPTIONAL

    public virtual void OnClickBackground()
    {
        Hide();
    }

    #endregion
}