using System;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent (typeof(TaskManager))]
public class ResourceCounter : MonoBehaviour
{
    public event Action ResourcesChanged;

    private int _minerals;
    private int _gas;
    private TaskManager _taskManager;

    public int Minerals => _minerals;
    public int Gas=> _gas;

    private void Awake()
    {
        _taskManager = GetComponent<TaskManager> ();
    }

    private void OnEnable()
    {
        _taskManager.TaskCompleted += HandleTask;
    }

    private void OnDisable()
    {
        _taskManager.TaskCompleted -= HandleTask;
    }

    private void HandleTask(Task task)
    {   
        if (task is ExtractTask)
        {
            Add(task as ExtractTask);
        }
        else
        {
            
        }
    }

    public void Add(ExtractTask task)
    {
        switch (task.ResourceType)
        {
            case ResourceType.Minerals:
            {
                _minerals++; 
                break;
            }
            case ResourceType.Gas:
            {
                _gas++;
                break;
            }
        }

        ResourcesChanged?.Invoke();
    }

    public bool TrySpend(int minerals,int gas)
    {
        if (_minerals >= minerals && _gas >= gas)
        {
            _minerals -= minerals;
            _gas -= gas;
            ResourcesChanged?.Invoke();
            return true;
        }

        return false;   
    }
}
