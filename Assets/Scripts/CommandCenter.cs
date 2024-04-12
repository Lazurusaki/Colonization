using System;
using System.Reflection;
using UnityEngine;

public enum Mode
{
    BuildWorkers,
    BuildCommandCenter
}

[RequireComponent(typeof(UnitBuilder), typeof(ResourceCounter), typeof(TaskManager))]

public class CommandCenter : MonoBehaviour, ISelectable
{
    [SerializeField] private UnitBuildCatalog _buildCatalog;

    public event Action<Mode> ModeChanged;

    private const int WorkerBuildIndex = 0;
    private const int CommandCenterBuildIndex = 1;

    private Mode _mode = Mode.BuildWorkers;
    private ResourceCounter _resourceCounter;
    private UnitBuilder _builder;
    private TaskManager _taskManager;

    private void Awake()
    {
        _builder = GetComponent<UnitBuilder>();
        _resourceCounter = GetComponent<ResourceCounter>();
        _taskManager = GetComponent<TaskManager>();
    }

    private void OnEnable()
    {
        _resourceCounter.ResourcesChanged += HandleModes;
    }

    private void OnDisable()
    {
        _resourceCounter.ResourcesChanged -= HandleModes;
    }

    private void HandleModes()
    {
        switch (_mode)
        {
            case Mode.BuildWorkers:
                {
                    BuildWorkersMode();
                    break;
                }
            case Mode.BuildCommandCenter:
                {
                    BuildCommandCenterMode();          
                    break;
                }
        }
    }

    private void Start()
    {
        _taskManager.enabled = true;
    }

    private void BuildWorkersMode()
    {
        Unit build = _buildCatalog.Units[WorkerBuildIndex].Prefab;

        if (build && CheckEnoughResources(WorkerBuildIndex))
        {
            _builder.Build(build, transform).SetParent(transform);
        }
    }

    private void BuildCommandCenterMode()
    {
        if (CheckEnoughResources(CommandCenterBuildIndex))
        {
            Flag flag = transform.GetComponentInChildren<Flag>();

            if (flag)
            {
                _taskManager.CreateBuildTask(flag.transform, _buildCatalog.Units[CommandCenterBuildIndex].Prefab);          
            }

            _mode = Mode.BuildWorkers;
        }
    }

    private bool CheckEnoughResources(int index)
    {
        int mineralsCost = _buildCatalog.Units[index].Cost.Minerals;
        int gasCost = _buildCatalog.Units[index].Cost.Gas;

        if (_resourceCounter.TrySpend(mineralsCost, gasCost))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeMode(Mode mode)
    {
        _mode = mode;
        ModeChanged?.Invoke(_mode);
    }
}

/*
Unit build = _buildCatalog.Units[CommandCenterBuildIndex].Prefab;

if (build)
{
    _builder.Build(build, transform);
}
*/