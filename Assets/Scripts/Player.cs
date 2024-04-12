using UnityEngine;

[RequireComponent (typeof(Selector), typeof(FlagPlacer))]
public class Player : MonoBehaviour
{
    private Selector _selector;
    private FlagPlacer _flagPlacer;
    private CommandCenter _commandCenter;

    private void Awake()
    {
        _selector = GetComponent<Selector>();
        _flagPlacer = GetComponent<FlagPlacer>();
    }

    private void OnEnable()
    {
        _selector.ObjectSelected += HandleSelection;
        _selector.GroundPointSelected += PlaceFlag;
    }

    private void OnDisable()
    {
        _selector.ObjectSelected -= HandleSelection;
        _selector.GroundPointSelected += PlaceFlag;
    }

    private void HandleSelection(Transform selected)
    {
        if (selected.TryGetComponent(out CommandCenter commandCenter)) 
        { 
            _commandCenter = commandCenter;
        }
    }

    private void PlaceFlag(Vector3 position)
    {
        if (_commandCenter)
        {
            Flag flag = _commandCenter.GetComponentInChildren<Flag>();

            if (!flag)
            {
                _flagPlacer.PlaceFlag(position).SetParent(_commandCenter.transform);      
            }
            else
            {
                flag.transform.position = position;
            }

            _commandCenter.ChangeMode(Mode.BuildCommandCenter);
            _commandCenter = null;
        }        
    }
}
