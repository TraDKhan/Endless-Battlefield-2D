public interface IStatusEffectReceiver
{
    void ApplyPoison(float duration, int dps);
    void ApplySlow(float duration, float slowPercent);
}
