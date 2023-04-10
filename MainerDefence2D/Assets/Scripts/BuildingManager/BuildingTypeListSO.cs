using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_buildingTypeList_", menuName = "Game/Building/Create Type List", order = 51)]
public class BuildingTypeListSO : ScriptableObject
{
    [SerializeField] private List<BuildingTypeSO> _buildingTypeList = new List<BuildingTypeSO>();

    public List<BuildingTypeSO> List => _buildingTypeList;
}
