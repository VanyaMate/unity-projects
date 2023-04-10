using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Building;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/buildingItem/create", order = 51)]
    public class SO_InventoryBuildingItem : SO_InventoryItem
    {
        [SerializeField] private BuildingItemObject _buildingPrefab;
        [SerializeField] private GameObject _buildingClearPrefab;

        public BuildingItemObject BuildingPrefab => _buildingPrefab;
        public GameObject BuildingClearPrefab => _buildingClearPrefab;
    }
}