using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public event Action TargetReached;

    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _targetDelta = 0.1f;

    private Vector3 _targetPosition;

    private void Update()
    {
        if (Vector3.Distance(transform.position,_targetPosition) > _targetDelta)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            transform.LookAt(_targetPosition);
            transform.position = newPosition;
        }
        else
        {
            TargetReached?.Invoke();
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
