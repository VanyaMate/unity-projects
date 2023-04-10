using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMCode.Inventory
{
    public class InventoryItemModel : MonoBehaviour
    {
        [SerializeField] private string _itemName;
        [SerializeField] private InventoryItem _inventoryItemController;

        public InventoryItem ItemController => _inventoryItemController;
    
        public void SetController(InventoryItem controller)
        {
            this._inventoryItemController = controller;
            this._itemName = controller.Type.Name;
        }
    }
}
