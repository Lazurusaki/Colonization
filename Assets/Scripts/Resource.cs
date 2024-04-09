using UnityEngine;

public enum ResourceType
{
    Minerals,
    Gas
}

public class Resource : MonoBehaviour, IPickupable
{
    [SerializeField] private ResourceType _type;

    public ResourceType Type => _type;
}
