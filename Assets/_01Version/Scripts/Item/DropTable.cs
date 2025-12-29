using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Drop Table")]
public class DropTable : ScriptableObject
{
    public List<DropEntry> drops;
}
