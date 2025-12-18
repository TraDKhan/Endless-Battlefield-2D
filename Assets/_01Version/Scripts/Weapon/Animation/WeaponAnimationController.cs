using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string fireTrigger = "Fire";
    [SerializeField] private string laserBool = "IsLaserFiring";

    public void PlayFire()
    {
        animator.SetTrigger(fireTrigger);
    }

    public void StartLaser()
    {
        animator.SetBool(laserBool, true);
    }

    public void StopLaser()
    {
        animator.SetBool(laserBool, false);
    }
}