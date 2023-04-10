using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceTypeSO _resourceType;

    public ResourceTypeSO Type => _resourceType;
}
