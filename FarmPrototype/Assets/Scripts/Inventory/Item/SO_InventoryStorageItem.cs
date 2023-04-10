using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/storageItem/create", order = 51)]
    public class SO_InventoryStorageItem : SO_InventoryItem
    {
        [SerializeField] private SO_InventoryManager _storageType;

        public SO_InventoryManager storageType => _storageType;
    }
}