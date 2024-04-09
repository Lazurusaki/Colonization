using System;
using UnityEngine;

[RequireComponent (typeof(TaskManager))]
public class ResourceCounter : MonoBehaviour
{
    public event Action<int, int> ResourcesChanged;

    private int _minerals;
    private int _gas;
    private TaskManager _taskManager;

    private void Awake()
    {
        _taskManager = GetComponent<TaskManager> ();
    }

    private void OnEnable()
    {
        _taskManager.TaskCompleted += Add;
    }

    private void OnDisable()
    {
        _taskManager.TaskCompleted -= Add;
    }

    // Update is called once per frame
    public void Add(Task task)
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

        ResourcesChanged?.Invoke(_minerals,_gas);
    }
}
