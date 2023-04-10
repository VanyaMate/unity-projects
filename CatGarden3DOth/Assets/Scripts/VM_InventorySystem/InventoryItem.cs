using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.InventoryManager
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private InventoryItemManager _manager;
        [SerializeField] private SO_InventoryData _inventoryData;
        [SerializeField] private float _amount;

        public InventoryItemManager Manager => _manager;

        private void Awake()
        {
            if (this._inventoryData != null)
            {
                this._manager = new InventoryItemManager(
                    this._inventoryData, 
                    this._amount != 0 ? this._amount : 1
                );

                this._manager.SetItemOnScene(transform);
            }
        }

        public void SetManager(InventoryItemManager manager)
        {
            this._manager = manager;
        }
    }
}