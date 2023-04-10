using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "buildingTypeListSO_", menuName = "Game/Building/SO/CreateList", order = 51)]
public class BuildingTypeListSO : ScriptableObject
{
    [SerializeField] private List<BuildingTypeSO> _buildingTypeList;

    public List<BuildingTypeSO> List => this._buildingTypeList;
}
