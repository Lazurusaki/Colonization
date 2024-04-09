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

    private void OnScoreChanged(int minerals,int gas)
    {
        _minerals.text = minerals.ToString();
        _gas.text = gas.ToString();
    }
}
