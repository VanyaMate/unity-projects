using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeHolder : MonoBehaviour
{
    [SerializeField] private BuildingTypeSO _buildingType;

    public BuildingTypeSO Type => _buildingType;
}
