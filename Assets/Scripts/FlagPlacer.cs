using System;
using UnityEngine;

[RequireComponent(typeof(Selector),typeof(InputDetector))]
public class FlagPlacer : MonoBehaviour
{
    public event Action<Transform> FlagPlaced;
    
    [SerializeField] private Transform _flagPrefab;

    private Selector _selector;
    private InputDetector _inputDetector;
    private bool _placeFlagModeOn;
    private Transform _flagParent;

    private void Awake()
    {
        _selector = GetComponent<Selector>();
        _inputDetector = GetComponent<InputDetector>();
    }
 
    private void OnEnable()
    {
        _selector.ObjectSelected += TurnPlaceFlagMode;
        _inputDetector.SelectButtonPressed += TryPlaceFlag;
    }

    private void OnDisable()
    {
        _selector.ObjectSelected -= TurnPlaceFlagMode;
        _inputDetector.SelectButtonPressed -= TryPlaceFlag;
    }

    private void TurnPlaceFlagMode(Transform transform)
    {
        if (transform.TryGetComponent(out CommandCenter commandCenter))
        {
            _flagParent = commandCenter.transform;
            _placeFlagModeOn = true;
        }
        else
        {
            _flagParent = null;
            _placeFlagModeOn = false;
        }
    }

    public void TryPlaceFlag()
    {
        if (_placeFlagModeOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent<IFlagPlacable>(out _))
            {
                Flag flag = _flagParent.gameObject.GetComponentInChildren<Flag>();

                if (!flag)
                {
                    Transform newFlag = Instantiate(_flagPrefab, hit.point, Quaternion.identity);
                    float randomAngleRange = 360.0f;
                    newFlag.Rotate(Vector3.up * UnityEngine.Random.Range(0, randomAngleRange));
                    newFlag.SetParent(_flagParent.transform);
                    FlagPlaced?.Invoke(_flagParent);             
                }
                else
                {
                    flag.transform.position = hit.point;     
                }

                _selector.Deselect();
                _placeFlagModeOn = false;
                _flagParent = null;
            }
        }
    }
}
