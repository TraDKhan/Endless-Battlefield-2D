using System.Collections.Generic;

public interface IStatSource
{
    IEnumerable<StatModifier> GetModifiers();
}
