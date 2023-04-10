using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_buildingType_list_", menuName = "game/building/create list", order = 51)]
public class BuildingTypeListSO : ScriptableObject
{
    [SerializeField] private List<BuildingTypeSO> _list = new List<BuildingTypeSO>();

    public List<BuildingTypeSO> List => _list;
}
