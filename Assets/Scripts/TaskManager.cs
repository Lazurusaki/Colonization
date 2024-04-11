using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scaner),typeof(CommandCenter))]
public class TaskManager : MonoBehaviour
{
    public event Action<Task> TaskCompleted;

    private Scaner _scaner;
    private List<Task> _tasks = new List<Task>();
    [SerializeField] private List<Worker> _workers = new List<Worker>();
    private UnitBuilder _unitBuilder;
    private bool _isBuildPrioritet = false;

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
        if (_tasks.Count > 0)
        {
            foreach (Task task in _tasks)
            {
                if (task is BuildUnitTask && task.InProgress == false)
                {
                    if (_workers.Count > 0)
                    {
                        Worker worker = _workers.Find(worker => worker.IsBusy == false);

                        if (worker)
                        {
                            task.ChangeStatus(true);
                            worker.TakeTask(task);
                        }
                    }
                    return;
                }
            }
        }

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

    private void CompleteTask(Task task, Worker worker)
    {
        if (task is BuildUnitTask)
        {
            _workers.Remove(worker);

        }

        _tasks.Remove(task);
        TaskCompleted?.Invoke(task);
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

    public void SetBuildPrioritet(bool set)
    {
        _isBuildPrioritet = set;
    }

    public void BuildBase(Transform transform,int unitIndex)
    {
        BuildUnitTask task = new BuildUnitTask(transform, _unitBuilder.Catalog.Units[unitIndex].Prefab);
        _tasks.Add(task);
    }
}
