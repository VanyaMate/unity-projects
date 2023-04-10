using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.TerrainTools;
using VM.UI;

namespace VM.Inventory.Items
{
    public class InventoryItemShovel : InventoryItem
    {
        private bool _active;

        public InventoryItemShovel(SO_InventoryItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._active = false;
        }

        public override void Activate()
        {
            this._active = true;
            MenuController.blockOpenMenu = true;
            TerrainManager.Instance.Enable();
        }

        public override void DeActivate()
        {
            this._active = false;
            TerrainManager.Instance.Disable();  
        }
    }
}