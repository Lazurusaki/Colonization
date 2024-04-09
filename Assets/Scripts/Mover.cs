using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;

    private Vector3 _targetPosition;

    private void Start()
    {
        _targetPosition = transform.position;   
    }

    void Update()
    {
        if (_targetPosition != transform.position)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            transform.LookAt(_targetPosition);
            transform.position = newPosition;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
