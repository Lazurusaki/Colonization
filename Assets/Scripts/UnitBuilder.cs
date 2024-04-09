using System;
using UnityEngine;

[RequireComponent (typeof(ResourceCounter))]

public class UnitBuilder : MonoBehaviour
{
    public event Action<Transform> UnitBuilt;

    [SerializeField] private UnitBuildCatalog _catalog;

    private ResourceCounter _resourceCounter;

    private void Awake()
    {
        _resourceCounter = GetComponent<ResourceCounter>();
    }

    public void TryBuildUnit(int index, Transform transform)
    {
        if (_catalog)
        {       
            int mineralsCost = _catalog.Units[index].Cost.Minerals;
            int gasCost = _catalog.Units[index].Cost.Gas;

            if (_resourceCounter.TrySpend(mineralsCost, gasCost))
            { 
                Transform unitTransform = Instantiate(_catalog.Units[index].Prefab, transform).transform;
                UnitBuilt?.Invoke(unitTransform);
            }
        }
    }
}
