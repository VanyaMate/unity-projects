using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.TerrainTools;
using VM.UI;
using VM.UI.Inventory;

namespace VM.Inventory.Items
{
    public class ShovelMode
    {
        public static int Change => 1;
        public static int Alignment => 2;
        public static int Destroy => 3;
    }

    public class InventoryItemShovel : InventoryItem
    {
        private int _mode;
        private List<HandsItemInfo> _handsMenuItems = new List<HandsItemInfo>()
        {
            new HandsItemInfo()
            {
                text = "~",
                action = () =>
                {
                    TerrainManager.Instance.SetShovelMode(ShovelMode.Destroy);
                }
            },
            new HandsItemInfo()
            {
                text = "+",
                action = () =>
                {
                    TerrainManager.Instance.SetShovelMode(ShovelMode.Change);
                }
            },
            new HandsItemInfo()
            {
                text = "=",
                action = () =>
                {
                    TerrainManager.Instance.SetShovelMode(ShovelMode.Alignment);
                }
            }
        };

        public bool filled;

        public InventoryItemShovel(SO_InventoryItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._active = false;
            this.filled = false;
            this._mode = ShovelMode.Change;
        }

        public override void Activate()
        {
            this._active = true;
            MenuController.blockOpenMenu = true;
            TerrainManager.Instance.EnableShovel(this);
            HandsManagerUI.instance.Show(this._handsMenuItems);
        }

        public override void DeActivate()
        {
            this._active = false;
            TerrainManager.Instance.DisableShovel();
            HandsManagerUI.instance.Hide();
        }
    }
}