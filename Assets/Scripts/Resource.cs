using UnityEngine;

public enum ResourceType
{
    Minerals,
    Gas
}

public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceType _type;

    public ResourceType Type => _type;
}
