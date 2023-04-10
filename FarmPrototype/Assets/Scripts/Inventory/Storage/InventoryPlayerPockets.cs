using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    public class InventoryPlayerPockets : InventoryManagerObject
    {
        public static InventoryPlayerPockets Instance;

        [SerializeField] private bool _isPlayerInventory;

        private void Awake()
        {
            if (this._isPlayerInventory)
            {
                Instance = this;
            }

            if (this._managerType != null)
            {
                this._manager = new InventoryManager(this._managerType);
            }
        }

        public void ResetStorage ()
        {
            for (int i = 0; i < this._manager.Inventory.Count; i++)
            {
                this._manager.Get(i);
            }
        }
    }
}