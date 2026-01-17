using UnityEngine;
using System.Collections;

public class UIWavePopupController : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private UIWaveView normalWaveView;
    [SerializeField] private UIWaveView bossWaveView;

    [Header("Timing")]
    [SerializeField] private float titleHoldTime = 2f;
    [SerializeField] private float countdownTime = 3f;

    [Header("Events")]
    [SerializeField] private WaveEventChannel waveEventChannel;

    private UIWaveView currentView;
    private Coroutine flowRoutine;

    void OnEnable()
    {
        waveEventChannel.OnWavePreview += OnWavePreview;
    }

    void OnDisable()
    {
        waveEventChannel.OnWavePreview -= OnWavePreview;
    }

    void OnWavePreview(WaveData wave, int waveIndex)
    {
        if (flowRoutine != null)
            StopCoroutine(flowRoutine);

        if (wave is BossWaveData)
            currentView = bossWaveView;
        else
            currentView = normalWaveView;

        flowRoutine = StartCoroutine(WaveFlow(wave, waveIndex));
    }

    IEnumerator WaveFlow(WaveData wave, int waveIndex)
    {
        normalWaveView.gameObject.SetActive(false);
        bossWaveView.gameObject.SetActive(false);

        currentView.gameObject.SetActive(true);
        currentView.Setup(wave, waveIndex);

        yield return currentView.PlayTitleIn();
        yield return new WaitForSeconds(titleHoldTime);
        yield return currentView.PlayTitleOut();
        yield return currentView.PlayCountdown(countdownTime);

        waveEventChannel?.OnWaveStart?.Invoke();
        currentView.gameObject.SetActive(false);
    }
}
