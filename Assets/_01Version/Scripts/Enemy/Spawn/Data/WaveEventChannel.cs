using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Wave Event Channel")]
public class WaveEventChannel : ScriptableObject
{
    public Action<WaveData, int> OnWavePreview;
    public Action OnWaveStart;
    public Action OnWaveCleared;
}
