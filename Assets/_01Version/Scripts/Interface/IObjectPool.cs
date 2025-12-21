using UnityEngine;

public interface IObjectPool
{
    Component Spawn();
    void Despawn(Component obj);
}
