using UnityEngine;

public class Task
{
    protected Transform _transform;
    protected bool _inProgress;
    
    public Transform Transform => _transform;
    public bool InProgress => _inProgress;
    
    public void ChangeStatus(bool status)
    {
        _inProgress = status;
    }
}
