using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeImpulse : MonoBehaviour
{
    public static CameraShakeImpulse Instance;

    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        Instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float strength = 1f)
    {
        // strength có thể dùng để scale amplitude
        impulseSource.GenerateImpulse(strength);
    }
}