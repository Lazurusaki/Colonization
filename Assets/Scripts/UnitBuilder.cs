using System;
using UnityEngine;

public class UnitBuilder : MonoBehaviour 
{
    public event Action<Transform> UnitBuilt;

    public Transform Build(Unit prefab, Transform transform)
    {  
        Transform buildTransform = Instantiate(prefab, transform.position, transform.rotation).transform;

        if (buildTransform)
        {
            UnitBuilt?.Invoke(buildTransform);
        }

        return buildTransform;
    }
}
