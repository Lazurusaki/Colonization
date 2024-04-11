using System;
using UnityEngine;

public enum Mode
{
    BuildWorkers,
    BuildCommandCenter
}

[RequireComponent(typeof(UnitBuilder), typeof(ResourceCounter), typeof(TaskManager))]

public class CommandCenter : MonoBehaviour, ISelectable, ISpawnBlocker
{
    public event Action<Mode> ModeChanged;

    private const int WorkerBuildIndex = 0;
    private const int CommandCenterBuildIndex = 1;

    private Mode _mode = Mode.BuildWorkers;
    private ResourceCounter _resourceCounter;
    private UnitBuilder _unitBuilder;
    private TaskManager _taskManager;
    private bool _isBaseBuildPrioritet = false;

    private void Awake()
    {
        _unitBuilder = GetComponent<UnitBuilder>();
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
                    _unitBuilder.TryBuildUnit(WorkerBuildIndex, transform);
                    break;
                }
            case Mode.BuildCommandCenter:
                {
                    BuildCommandCenter();
                    break;
                }
        }
    }

    private void BuildCommandCenter()
    {
        if (_unitBuilder.CheckEnoughResources(CommandCenterBuildIndex) && !_isBaseBuildPrioritet)
        {
            Flag flag = transform.GetComponentInChildren<Flag>();

            if (flag)
            {
                _isBaseBuildPrioritet = true;
                _taskManager.BuildBase(flag.transform, CommandCenterBuildIndex);
                _mode = Mode.BuildWorkers;
            }
        }
    }

    public void ChangeMode(Mode mode)
    {
        _mode = mode;
        ModeChanged?.Invoke(_mode);
    }    
}
