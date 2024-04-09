using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(UnitBuilder),typeof(ResourceCounter))]

public class Base : MonoBehaviour
{
    private const int WorkerIndex = 0;

    private List<Worker> _workers = new List<Worker>();
    private ResourceCounter _resourceCounter;
    private UnitBuilder _unitBuilder;
    
    private void Awake()
    {
        _unitBuilder = GetComponent<UnitBuilder>();
        _resourceCounter = GetComponent<ResourceCounter>();
    }

    private void OnEnable()
    {
        _resourceCounter.ResourcesChanged += TryBuildUnit;
    }

    private void OnDisable()
    {
        _resourceCounter.ResourcesChanged -= TryBuildUnit;
    }

    private void TryBuildUnit()
    {
        _unitBuilder.TryBuildUnit(WorkerIndex, transform);
    }
}
