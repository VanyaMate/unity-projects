using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.TerrainTools;
using VM.UI;
using VM.UI.Inventory;

namespace VM.Inventory.Items
{
    public class InventoryItemScythe : InventoryItem
    {
        public InventoryItemScythe(SO_InventoryItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {

        }

        public override void Activate()
        {
            MenuController.blockOpenMenu = true;
            TerrainManager.Instance.EnableScythe(this);
        }

        public override void DeActivate()
        {
            TerrainManager.Instance.DisableScythe();
            HandsManagerUI.instance.Hide();
        }
    }
}