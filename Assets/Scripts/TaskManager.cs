using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner))]
public class TaskManager : MonoBehaviour
{
    public event Action<Task> TaskCompleted;

    private Scaner _scaner;
    private List<Task> _tasks = new List<Task>();
    private List<Worker> _workers = new List<Worker>();

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
    }

    private void OnEnable()
    {
        _scaner.ResourceDetected += HandleTask;

        Worker[] workers = GetComponentsInChildren<Worker>();

        if (workers.Length > 0)
        {
            foreach (Worker worker in workers)
            {
                _workers.Add(worker);
                worker.ReadyToWork += TryGiveTask;
                worker.TaskCompleted += CompleteTask;
            }
        }
    }

    private void OnDisable()
    {
        _scaner.ResourceDetected -= HandleTask;
        
        if (_workers.Count > 0 ) 
        {
            foreach (Worker worker in _workers) 
            {
                worker.ReadyToWork -= TryGiveTask;
                worker.TaskCompleted -= CompleteTask;
            }
        }
    }

    private void HandleTask(Transform transform)
    {
        TryAddTask(transform);

        if (_tasks.Count > 0)
        {
            if (_workers.Count > 0)
            {
                Worker worker = _workers.Find(worker => worker.IsBusy == false);

                if (worker)
                {
                    TryGiveTask(worker);
                }
            }
        }
    }

    private void TryAddTask(Transform transform)
    {
        if (_tasks.Count > 0)
        {
            foreach (Task task in _tasks)
            {        
                if (task.Transform == transform)
                {
                    return;
                }
            }     
        }

        AddTask(transform);
    }
 
    private void TryGiveTask(Worker worker)
    {
        if (_tasks.Count > 0)
        {
            foreach(Task task in _tasks)        
            {
                if (task.InProgress == false)
                {
                    task.ChangeStatus(true);
                    worker.TakeTask(task);
                    break;
                }
            }   
        }
    }

    private void CompleteTask(Task task)
    {
        _tasks.Remove(task);
        TaskCompleted?.Invoke(task);
    }
    private void AddTask(Transform transform)
    {
        Task task = new Task(transform);
        _tasks.Add(task);
    }
}
