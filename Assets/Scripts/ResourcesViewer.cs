using TMPro;
using UnityEngine;

public class ResourcesViewer : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private TMP_Text _minerals;
    [SerializeField] private TMP_Text _gas;

    private void OnEnable()
    {
        _resourceCounter.ResourcesChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _resourceCounter.ResourcesChanged -= OnScoreChanged;
    }

    private void OnScoreChanged()
    {
        _minerals.text = _resourceCounter.Minerals.ToString();
        _gas.text = _resourceCounter.Gas.ToString();
    }
}
