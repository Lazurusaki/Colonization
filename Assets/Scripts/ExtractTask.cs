using UnityEngine;

public class ExtractTask : Task
{
    private ResourceType _resourceType;
    private Vector3 _initialPosition; 

    public ResourceType ResourceType => _resourceType;
    public Vector3 InitialPosition => _initialPosition;

    public ExtractTask(Transform transform)
    {
        transform.gameObject.TryGetComponent(out Resource resource);

        if (resource)
        {
            _resourceType = resource.Type;
        }

        _transform = transform;
        _initialPosition = _transform.position;
        _inProgress = false;
    }

}
