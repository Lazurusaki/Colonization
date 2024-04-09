using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public event Action<Transform> ResourceSpawned;

    [SerializeField] private float _minDistance = 5.0f;
    [SerializeField] private float _maxDistance = 30.0f;
    [SerializeField] private float _minFrequency = 3.0f;
    [SerializeField] private float _maxFrequency = 6.0f;
    [SerializeField] private List<Resource> _resourcePrefabs = new List<Resource>();

    private void OnValidate()
    {
        if (_maxDistance < _minDistance)
        {
            _maxDistance = _minDistance;
        }

        if (_maxFrequency < _minFrequency)
        {
            _maxFrequency = _minFrequency;
        }
    }

    private void Start()
    {
        if (_resourcePrefabs.Count > 0)
        {
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(UnityEngine.Random.Range(_minFrequency,_maxFrequency));

        while (true)
        {
            Resource resourcePrefab = _resourcePrefabs[UnityEngine.Random.Range(0, _resourcePrefabs.Count)];
            Resource resource = Instantiate(resourcePrefab, transform);
            float randomDistance = UnityEngine.Random.Range(_minDistance, _maxDistance);
            Vector3 randomAngle = UnityEngine.Random.rotation.eulerAngles;
            Vector3 randomOffset = new Vector3(Mathf.Cos(randomAngle.y), 0f, Mathf.Sin(randomAngle.y)) * randomDistance;
            resource.transform.position += randomOffset;
            Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.rotation.y, 0);
            resource.transform.rotation = rotation;
            ResourceSpawned?.Invoke(resource.transform);
            yield return wait;
        }
    }
}
