using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Database")]
public class PlayerDatabase : ScriptableObject
{
    public List<PlayerData> players;

    public PlayerData GetByID(int id)
    {
        return players.Find(p => p.id == id);
    }
}