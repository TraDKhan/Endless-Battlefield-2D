using System.Collections.Generic;

public interface IStatSource
{
    List<StatModifier> GetModifiers();
}
