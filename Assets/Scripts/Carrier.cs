using System;
using UnityEditor;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    public event Action Loaded;

    [SerializeField] private Transform _pickupSocket;

    private Transform _cargo;
    public Transform Cargo => _cargo;


    public void Take(Transform item)
    {
        if (!_cargo && _pickupSocket)
        {
            item.position = _pickupSocket.position;
            item.SetParent(transform);
            _cargo = item;
            Loaded?.Invoke();
        }       
    }

    public void Release()
    {
        if (_cargo)
        { 
            _cargo.transform.parent = null;
            _cargo = null;
        }
    }
}