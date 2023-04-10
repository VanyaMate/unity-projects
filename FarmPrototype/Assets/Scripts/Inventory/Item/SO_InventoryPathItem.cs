using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Managers.Path;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/path/create", order = 51)]
    public class SO_InventoryPathItem : SO_InventoryItem
    {
        [SerializeField] private PathNode _pathNodePrefab;

        public PathNode pathNodePrefab => _pathNodePrefab;
    }
}