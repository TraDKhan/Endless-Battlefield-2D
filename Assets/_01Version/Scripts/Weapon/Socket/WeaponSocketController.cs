using UnityEngine;
using System.Collections.Generic;

public class WeaponSocketController : MonoBehaviour
{
    public static WeaponSocketController Instance;

    [System.Serializable]
    public class Socket
    {
        public WeaponSlotType type;
        public Transform point;
    }

    public List<Socket> sockets;

    private Dictionary<WeaponSlotType, Transform> socketMap;

    private void Awake()
    {
        Instance = this;

        socketMap = new Dictionary<WeaponSlotType, Transform>();
        foreach (var s in sockets)
        {
            socketMap[s.type] = s.point;
        }
    }

    public Transform GetSocket(WeaponSlotType type)
    {
        return socketMap[type];
    }
}
