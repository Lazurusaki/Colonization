using System;
using UnityEngine;

[Serializable]
public struct Cost
{
    public int Minerals;
    public int Gas;
}

[CreateAssetMenu(menuName = "ScriptableObjects/UnitBuildData")]
public class UnitBuildData : ScriptableObject
{
    public Unit Prefab;
    public Cost Cost;
}
