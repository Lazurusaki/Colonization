using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public event Action<Transform> ResourceSpawned;

    [SerializeField] private float _mapQuadrantSize= 30.0f;
    [SerializeField] private float _minFrequency = 3.0f;
    [SerializeField] private float _maxFrequency = 6.0f;
    [SerializeField] private List<Resource> _resourcePrefabs = new List<Resource>();

    private void OnValidate()
    {
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
            float xPos = UnityEngine.Random.Range(-_mapQuadrantSize, _mapQuadrantSize);
            float yPos = UnityEngine.Random.Range(-_mapQuadrantSize, _mapQuadrantSize);
            Resource resource = Instantiate(resourcePrefab, transform);
            resource.transform.position = new Vector3(xPos,0.0f,yPos);
            Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.rotation.y, 0);
            resource.transform.rotation = rotation;
            ResourceSpawned?.Invoke(resource.transform);
            yield return wait;
        }
    }
}
