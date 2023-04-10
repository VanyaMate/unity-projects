using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/robot/create", order = 51)]
    public class SO_InventoryRobotItem : SO_InventoryItem
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _takeSeedlingAmount;
        [SerializeField] private InventoryItemObject _robotItemObject;
        [SerializeField] private SO_InventoryManager _robotInventory;

        public float speed => _speed;
        public float takeSeedlingAmount => _takeSeedlingAmount;
        public InventoryItemObject robotItemObject => _robotItemObject;
        public SO_InventoryManager robotInventoryType => _robotInventory;
    }
}