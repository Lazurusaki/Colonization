using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UnitBuildCatalog")]
public class UnitBuildCatalog : ScriptableObject
{
    public List<UnitBuildData> Units = new List<UnitBuildData>();
}


