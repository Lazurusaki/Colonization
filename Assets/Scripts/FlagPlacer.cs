using System;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    public event Action<Transform> FlagPlaced;
    
    [SerializeField] private Flag _flagPrefab;

    public Transform PlaceFlag(Vector3 position)
    {
        if (_flagPrefab)
        {
            Transform flag = Instantiate(_flagPrefab, position, Quaternion.identity).transform;
            float randomAngleRange = 360.0f;
            flag.Rotate(Vector3.up * UnityEngine.Random.Range(0, randomAngleRange));
            FlagPlaced?.Invoke(flag);     
            return flag;
        }
        else
        {
            return null;
        }
    }
}
