using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_resourceTypeList_", menuName = "Game/Resource/Create Type List", order = 51)]
public class ResourceTypeListSO : ScriptableObject
{
    [SerializeField] private List<ResourceTypeSO> _resourceTypes = new List<ResourceTypeSO>();

    public List<ResourceTypeSO> List => _resourceTypes;
}
