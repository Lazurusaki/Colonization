using System;
using UnityEngine;

[RequireComponent (typeof(InputDetector))]
public class Selector : MonoBehaviour
{
    public event Action<Transform> ObjectSelected;
    public event Action<Vector3> GroundPointSelected;

    [SerializeField] private Canvas selectorUIPrefab;

    private InputDetector _inputDetector;
    private Transform _selectedTransform;
    private Canvas _selectorUI;

    private void Awake()
    {
        _inputDetector = GetComponent<InputDetector>();
    }

    private void OnEnable()
    {
        _inputDetector.SelectButtonPressed += Select;
    }
    private void OnDisable()
    {
        _inputDetector.SelectButtonPressed -= Select;
    }

    private void Start()
    {
        _selectorUI = Instantiate(selectorUIPrefab,transform);
        _selectorUI.gameObject.SetActive(false);
    }

    private void Select()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(ray, out RaycastHit hit);

        if (isHit)
        {
            if (hit.collider.TryGetComponent<ISelectable>(out _))
            {
                Transform transform = hit.collider.gameObject.transform;
                _selectedTransform = transform;
                _selectorUI.transform.position = _selectedTransform.position;
                _selectorUI.gameObject.SetActive(true);
                ObjectSelected?.Invoke(transform);
            }
            else if (hit.collider.TryGetComponent<Ground>(out _))
            {
                if (_selectedTransform)
                {            
                    GroundPointSelected?.Invoke(hit.point);
                    _selectorUI.gameObject.SetActive(!_selectedTransform);
                    _selectedTransform = null;
                }
            }
        }
        
        else
        {
            if (_selectedTransform)
            {
                _selectorUI.gameObject.SetActive(!_selectedTransform);
                _selectedTransform = null;
            }
        }
    }
}
