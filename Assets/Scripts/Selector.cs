using System;
using UnityEngine;

[RequireComponent (typeof(InputDetector))]
public class Selector : MonoBehaviour
{
    public event Action<Transform> ObjectSelected;

    [SerializeField] private Canvas selectorUIPrefab;

    private InputDetector _inputDetector;
    private Transform _selectedObject;
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

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent<ISelectable>(out _))
        {
            Transform transform = hit.collider.gameObject.transform;
            _selectedObject = transform;
            _selectorUI.transform.position = _selectedObject.position;
            _selectorUI.gameObject.SetActive(true);
            ObjectSelected?.Invoke(transform);
        }
        else
        {
            if (_selectedObject)
            {
                _selectedObject = null;
                _selectorUI.gameObject.SetActive(false);
            }
        }
    }

    public void Deselect()
    {
        _selectedObject = null;
        _selectorUI.gameObject.SetActive(false);
    }
}
