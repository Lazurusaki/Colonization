using System;
using UnityEngine;

public class Task
{
    private Transform _transform;
    private  bool _inProgress;
    private ResourceType _resourceType;

    public Transform Transform => _transform;
    public bool InProgress => _inProgress;
    public ResourceType ResourceType => _resourceType;
    
    public Task(Transform transform)
    {
        transform.gameObject.TryGetComponent(out Resource resource);

        if (resource)
        {
            _resourceType = resource.Type;
        }

        _transform = transform;
        _inProgress = false;
    }

    public void ChangeStatus(bool status)
    {
        _inProgress = status;
    }
}
