using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class UIWaveBossView : UIWaveView
{
    [SerializeField] private RectTransform titleGroup;
    [SerializeField] private RectTransform countdownGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    //[SerializeField] private TextMeshProUGUI waveNameText;
    [SerializeField] private TextMeshProUGUI countdownText;

    public override void Setup(WaveData wave, int index)
    {
        titleText.text = "BOSS";
        //waveNameText.text = wave.waveName.ToUpper();
    }

    public override IEnumerator PlayTitleIn()
    {
        titleGroup.localScale = Vector3.one * 0.6f;

        Sequence seq = DOTween.Sequence();
        seq.Append(titleGroup.DOScale(1.1f, 0.3f));
        seq.Append(titleGroup.DOShakePosition(0.4f, 25f));

        yield return seq.WaitForCompletion();
    }

    public override IEnumerator PlayTitleOut()
    {
        yield return titleGroup
            .DOScale(0f, 0.25f)
            .SetEase(Ease.InBack)
            .WaitForCompletion();
    }

    public override IEnumerator PlayCountdown(float time)
    {
        countdownGroup.gameObject.SetActive(true);

        for (int i = (int)time; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownGroup.localScale = Vector3.one * 1.4f;
            countdownGroup.DOScale(1f, 0.15f).SetEase(Ease.OutElastic);

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "FIGHT!";
        yield return countdownGroup.DOScale(1.8f, 0.25f).WaitForCompletion();
        countdownGroup.gameObject.SetActive(false);
    }
}
