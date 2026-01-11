using System.Collections;
using UnityEngine;

/// <summary>
/// Hiệu ứng chảy máu
/// </summary>
public class BleedEffect : MonoBehaviour
{
    Coroutine bleedRoutine;

    public void Apply(int damagePerTick, float duration)
    {
        if (bleedRoutine != null)
            StopCoroutine(bleedRoutine);

        bleedRoutine = StartCoroutine(Bleed(damagePerTick, duration));
    }

    IEnumerator Bleed(int dmg, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            GetComponent<IDamageable>()?.TakeDamage(dmg);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }
}
