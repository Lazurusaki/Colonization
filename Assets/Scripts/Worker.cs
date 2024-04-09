using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Carrier))]

public class Worker : MonoBehaviour
{
    public event Action<Worker> ReadyToWork;
    public event Action<Task> TaskCompleted;

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
    }

    private void OnDisable()
    {
        _carrier.Loaded -= ReturnToBase;
        _carrier.Delivered -= FinishWork;
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
        TaskCompleted?.Invoke(_task);
        _isBusy = false;
        ReadyToWork?.Invoke(this);
    }
}
