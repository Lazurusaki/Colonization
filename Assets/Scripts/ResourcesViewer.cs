using TMPro;
using UnityEngine;

public class ResourcesViewer : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private TMP_Text _minerals;
    [SerializeField] private TMP_Text _gas;
    [SerializeField] private Vector3 _rotation;

    private Canvas _canvas;

    private void OnEnable()
    {
        _resourceCounter.ResourcesChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _resourceCounter.ResourcesChanged -= OnScoreChanged;
    }

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnScoreChanged()
    {
        _minerals.text = _resourceCounter.Minerals.ToString();
        _gas.text = _resourceCounter.Gas.ToString();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(_rotation);
    }
}
