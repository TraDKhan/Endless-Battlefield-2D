using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class UIWaveNormalView : UIWaveView
{
    [SerializeField] private RectTransform titleGroup;
    [SerializeField] private RectTransform countdownGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    //[SerializeField] private TextMeshProUGUI waveNameText;
    [SerializeField] private TextMeshProUGUI countdownText;

    public override void Setup(WaveData wave, int index)
    {
        titleText.text = $"{wave.waveName.ToUpper()} + {index}";
        //waveNameText.text = $"Wave {index}: {wave.waveName}";
    }

    public override IEnumerator PlayTitleIn()
    {
        titleGroup.anchoredPosition = new Vector2(0, 120);
        titleGroup.localScale = Vector3.one * 0.9f;

        Sequence seq = DOTween.Sequence();
        seq.Append(titleGroup.DOAnchorPosY(0, 0.4f).SetEase(Ease.OutBack));
        seq.Join(titleGroup.DOScale(1f, 0.3f));

        yield return seq.WaitForCompletion();
    }

    public override IEnumerator PlayTitleOut()
    {
        yield return titleGroup
            .DOAnchorPosY(80, 0.25f)
            .SetEase(Ease.InQuad)
            .WaitForCompletion();
    }

    public override IEnumerator PlayCountdown(float time)
    {
        countdownGroup.gameObject.SetActive(true);

        for (int i = (int)time; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownGroup.localScale = Vector3.one * 1.2f;
            countdownGroup.DOScale(1f, 0.2f);

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return countdownGroup.DOScale(1.5f, 0.25f).WaitForCompletion();
        countdownGroup.gameObject.SetActive(false);
    }
}
