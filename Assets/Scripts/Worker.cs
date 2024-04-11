using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Carrier))]
public class Worker : MonoBehaviour
{
    public event Action<Worker> ReadyToWork;
    public event Action<Task,Worker> TaskCompleted;

    private Task _task;
    private Carrier _carrier;
    private Mover _mover;
    private bool _isBusy = false; 

    public Task Task => _task;
    public bool IsBusy => _isBusy;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _carrier = GetComponent<Carrier>();
    }

    private void OnEnable()
    {
        _carrier.Loaded += ReturnToBase;
        _carrier.Delivered += FinishWork;
        _mover.TargetReached += CheckTask;
    }

    private void OnDisable()
    {
        _carrier.Loaded -= ReturnToBase;
        _carrier.Delivered -= FinishWork;
        _mover.TargetReached -= CheckTask;
    }

    private void Start()
    {
        ReadyToWork?.Invoke(this);
    }
    
    public void TakeTask(Task task)
    {
        _task = task;
        _mover.SetTargetPosition(task.Transform.position);
        _isBusy = true;
    }

    public void ReturnToBase()
    {
        _mover.SetTargetPosition(transform.parent.position);
    }

    public void FinishWork()
    { 
        TaskCompleted?.Invoke(_task,this);
        _isBusy = false;
        ReadyToWork?.Invoke(this);
    }

    public void CheckTask()
    {
        if (_task is BuildUnitTask)
        {
            Unit newUnit = Instantiate((_task as BuildUnitTask).Unit,_task.Transform.position, Quaternion.identity); //BuildUnit     
            TaskCompleted?.Invoke(_task,this);
            transform.SetParent(newUnit.transform);

            if (newUnit.TryGetComponent(out TaskManager taskManager))
            {
                taskManager.TryAddWorker(transform);
            }
                 
        }

        _task = null;
        _isBusy = false;
        ReadyToWork?.Invoke(this);
    }
}
