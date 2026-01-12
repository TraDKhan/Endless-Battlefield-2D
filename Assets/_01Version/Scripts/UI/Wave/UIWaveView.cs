using System.Collections;
using UnityEngine;

public abstract class UIWaveView : MonoBehaviour
{
    public abstract void Setup(WaveData wave, int waveIndex);

    public abstract IEnumerator PlayTitleIn();
    public abstract IEnumerator PlayTitleOut();
    public abstract IEnumerator PlayCountdown(float time);
}
