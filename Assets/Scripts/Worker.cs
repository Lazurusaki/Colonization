using System;
using UnityEngine;

[RequireComponent(typeof(Carrier),typeof(Mover))]
public class Worker : MonoBehaviour
{
    public event Action<Worker> ReadyToWork;
    public event Action<Task,Worker> TaskCompleted;

    private Task _task;
    private Carrier _carrier;
    private Mover _mover;

    public Task Task => _task;

    private void Awake()
    {
        _carrier = GetComponent<Carrier>();
        _mover = GetComponent<Mover>();
    }

    private void OnEnable()
    {
        _carrier.Loaded += ReturnToBase;
        _mover.TargetReached += HandleTask;
    }

    private void OnDisable()
    {
        _carrier.Loaded -= ReturnToBase;
        _mover.TargetReached += HandleTask;
    }

    private void Start()
    {
        ReadyToWork?.Invoke(this);
    }
    
    private void ReturnToBase()
    {
        _mover.SetTargetPosition(transform.parent.position);
    }

    private void HandleTask()
    {
        if (_task is ExtractTask)
        {
            if (!_carrier.Cargo)
            {
                if (_task.Transform != null && (_task.Transform.position == (_task as ExtractTask).InitialPosition))
                {
                    _carrier.Take(_task.Transform);
                }
                else
                {
                    Clear();
                }
            }
            else
            {
                TaskCompleted?.Invoke(_task, this);
                _carrier.Release();
                Clear();
            }
        }
        else if (_task is BuildUnitTask)
        {
            if (transform.TryGetComponent(out UnitBuilder _unitBuilder))
            {
                Transform build = _unitBuilder.Build((_task as BuildUnitTask).Unit, _task.Transform);

                if (build)
                {
                    transform.SetParent(build.transform);
                    TaskCompleted?.Invoke(_task, this);                
                }

                Clear();
            }
        }
    }

    private void Clear()
    {
        _task = null;
        ReadyToWork?.Invoke(this);
    }

    public void TakeTask(Task task)
    {
        _task = task;
        _mover.SetTargetPosition(_task.Transform.position);
    }
}
