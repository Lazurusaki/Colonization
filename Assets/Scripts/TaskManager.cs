using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Scaner), typeof(CommandCenter))]
public class TaskManager : MonoBehaviour
{
    public event Action<Task> TaskCompleted;

    private Scaner _scaner;
    private List<Task> _tasks = new List<Task>();
    [SerializeField] private List<Worker> _workers = new List<Worker>();
    private UnitBuilder _unitBuilder;

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
        _unitBuilder = GetComponent<UnitBuilder>();
    }

    private void OnEnable()
    {
        _scaner.ResourceDetected += HandleTask;
        _unitBuilder.UnitBuilt += TryAddWorker;

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
        _unitBuilder.UnitBuilt -= TryAddWorker;

        if (_workers.Count > 0)
        {
            foreach (Worker worker in _workers)
            {
                worker.ReadyToWork -= TryGiveTask;
                worker.TaskCompleted -= CompleteTask;
            }
        }
    }

    private void HandleTask(Transform transfrom)
    {
        TryAddTask(transfrom);

        foreach (Worker worker in _workers)
        {
            if (worker.Task == null)
            {
                TryGiveTask(worker);
            }
        }
    }

    private void TryAddTask(Transform transform)
    {
        foreach (Task task in _tasks)
        {
            if (task.Transform == transform)
            {
                return;
            }
        }

        AddTask(transform);
    }

    private void TryGiveTask(Worker worker)
    {
        _tasks.RemoveAll(task => task.Transform == null);

        foreach (Task task in _tasks)
        {
            if (task is BuildUnitTask && task.InProgress == false)
            {
                task.ChangeStatus(true);
                worker.TakeTask(task);
                worker.AddComponent<UnitBuilder>();
                return;
            }
        }

        foreach (Task task in _tasks)
        {
            if (task.InProgress == false)
            {
                task.ChangeStatus(true);
                worker.TakeTask(task);
                break;
            }
        }
    }

    private void CompleteTask(Task task, Worker worker)
    {
        if (task is BuildUnitTask)
        {
            if (worker.transform.TryGetComponent(out UnitBuilder unitBuilder))
            {
                Destroy(unitBuilder);
            }
     
            _workers.Remove(worker);
        }

        _tasks.Remove(task);

        if (task.Transform != null)
        {
            TaskCompleted?.Invoke(task);
        }
    }

    public void TryAddWorker(Transform transform)
    {
        if (transform.gameObject.TryGetComponent(out Worker worker))
        {
            AddWorker(worker);
        }
    }

    private void AddTask(Transform transform)
    {
        Task task = new ExtractTask(transform);
        _tasks.Add(task);
    }

    private void AddWorker(Worker worker)
    {
        _workers.Add(worker);
        worker.ReadyToWork += TryGiveTask;
        worker.TaskCompleted += CompleteTask;
    }

    public void CreateBuildTask(Transform transform,Unit prefab)
    {      
        BuildUnitTask task = new BuildUnitTask(transform, prefab);
        _tasks.Add(task);
    }
}
