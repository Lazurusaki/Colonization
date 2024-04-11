using System;
using UnityEngine;

[RequireComponent(typeof(Worker))]

public class Carrier : MonoBehaviour
{
    public event Action Delivered;
    public event Action Loaded;

    [SerializeField] private Transform _pickupSocket;

    private Worker _worker;
    private bool _isLoaded;

    private void Awake()
    {
        _worker = GetComponent<Worker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isLoaded)
        {
            if (_pickupSocket)
            {
                if (other.TryGetComponent<IPickupable>(out _) && 
                    other.transform.parent.TryGetComponent<ResourceSpawner>(out _) &&
                    other.transform.position == _worker.Task.Transform.position)
                {
                    other.transform.position = _pickupSocket.position;
                    other.transform.SetParent(transform);
                    _isLoaded = true;
                    Loaded?.Invoke();
                }
            }
        }  
        else
        {
            if (other.TryGetComponent<CommandCenter>(out _) && (other.transform == transform.parent))
            {
                Delivered?.Invoke();
                _isLoaded = false;
            }
        }
    }
}
