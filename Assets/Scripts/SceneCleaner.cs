using UnityEngine;

public class SceneCleaner : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;

    private void OnEnable()
    {
        if (_taskManager)
        {
            _taskManager.TaskCompleted += Clean;
        }
    }

    private void OnDisable()
    {
        if (_taskManager)
        {
            _taskManager.TaskCompleted -= Clean;
        }
    }

    private void Clean(Task task)
    {
         Destroy(task.Transform.gameObject);    
    }

    public void Clean(Transform transform)
    {
        Destroy(transform.gameObject);
    }
}
