using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drop/Drop Table")]
public class DropTable : ScriptableObject
{
    public List<DropEntry> drops;
}

[Serializable]
public class DropEntry
{
    public GameObject itemPrefab;
    [Range(0, 1)]
    public float dropChance = 0.5f;

    public int amount = 1;
}