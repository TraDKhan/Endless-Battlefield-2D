using System.Collections.Generic;

public interface IStatSource<TStat>
{
    IEnumerable<StatModifier<TStat>> GetModifiers();
}
