using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "resourseTypeListSO_", menuName = "Game/Resource/SO/CreateList", order = 51)]
public class ResourceTypeListSO : ScriptableObject
{
    [SerializeField] private List<ResourceTypeSO> _list;

    public List<ResourceTypeSO> List => _list;
}
