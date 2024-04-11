using UnityEngine;

public class BuildUnitTask : Task
{
    private Unit _unit;

    public Unit Unit => _unit;

    public BuildUnitTask(Transform transform, Unit unitPrefab)
    {   
        _unit = unitPrefab;
        _transform = transform;
        _inProgress = false;
    }
}
