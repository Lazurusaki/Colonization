using UnityEngine;

[RequireComponent (typeof(FlagPlacer),typeof(Selector))]
public class ModeChanger : MonoBehaviour
{
    private FlagPlacer _flagPlacer;

    private void Awake()
    {
        _flagPlacer = GetComponent<FlagPlacer>();
    }

    private void OnEnable()
    {
        _flagPlacer.FlagPlaced += ChangeMode;
    }

    private void OnDisable()
    {
        _flagPlacer.FlagPlaced += ChangeMode;
    }

    private void ChangeMode(Transform transform)
    {
        if(transform.gameObject.TryGetComponent(out CommandCenter commandCenter))
        {
            commandCenter.ChangeMode(Mode.BuildCommandCenter);       
        }       
    }
}
