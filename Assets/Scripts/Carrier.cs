using System;
using UnityEngine;

[RequireComponent(typeof(Worker))]

public class Carrier : MonoBehaviour
{
    public event Action Delivered;
    public event Action Loaded;

    [SerializeField] private Transform pickupSocket;

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
            if (pickupSocket)
            {
                if (other.TryGetComponent<IPickupable>(out _) && 
                    other.transform.parent.TryGetComponent<ResourceSpawner>(out _) &&
                    other.transform.position == _worker.Task.Transform.position)
                {
                    other.transform.position = pickupSocket.position;
                    other.transform.SetParent(transform);
                    _isLoaded = true;
                    Loaded?.Invoke();
                }
            }
        }  
        else
        {
            if (other.TryGetComponent<Base>(out _))
            {
                Delivered?.Invoke();
                _isLoaded = false;
            }
        }
    }
}
