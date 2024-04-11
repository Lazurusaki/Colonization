using UnityEngine;

public class ExtractTask : Task
{
    private ResourceType _resourceType;
    public ResourceType ResourceType => _resourceType;

    public ExtractTask(Transform transform)
    {
        transform.gameObject.TryGetComponent(out Resource resource);

        if (resource)
        {
            _resourceType = resource.Type;
        }

        _transform = transform;
        _inProgress = false;
    }

}
