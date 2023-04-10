using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers;
using VM.Seeds;
using VM.UI;
using VM.UI.WindowInfo;

namespace VM.Inventory.Items
{
    public class InventoryItemSeed : InventoryItem
    {
        protected new SO_InventorySeedsItem _itemType;
        private bool _active;

        public InventoryItemSeed(SO_InventorySeedsItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._itemType = type;
            this._active = false;
        }

        public new InventoryItemSeedling PlaceOnScene(Vector3 position, Quaternion rotation)
        {
            GameObject seedlingModel = MonoBehaviour.Instantiate(
                this._itemType.seedlingPrefab.gameObject,
                position,
                rotation
            );

            InventoryItemSeedling seedling = new InventoryItemSeedling(this._itemType.seedlingItemType, 1, seedlingModel);
            seedling.placed = true;

            seedlingModel.GetComponent<SeedsItemObject>().SetManager(seedling);
            seedling.AddToSeedlingManager();

            // переместить объект на позицию спауна
            seedlingModel.transform.parent = SeedlingsManager.instance.container;
            seedlingModel.transform.position = position;
            seedlingModel.transform.rotation = rotation;

            float seedSize = seedling.progress > 100 ? 1 : seedling.progress / 100; // seedling.progress <= 10 ? .05f : Mathf.Floor(seedling.progress / 10) / 10;
            seedling.seedsItemObject.transform.localScale = new Vector3(seedSize, seedSize, seedSize);

            return seedling;
        }

        public override void Activate()
        {
            this._active = true;
            SeedlingsManager.instance.ShowGhost(this);
        }

        public override void DeActivate()
        {
            this._active = false;
            SeedlingsManager.instance.HideGhost();
        }

        protected override void _OpenInfo()
        {
            Window window = UserInterface.Instance.OpenWindow(this._itemType.Name);
            WindowItemInfo itemInfo = MonoBehaviour.Instantiate(
                UserInterface.Instance.ItemInfo,
                window.WindowContainer
            );

            itemInfo.SetData(this._itemType.Icon, this._itemType.Name, new List<ItemPointData>()
            {
                new ItemPointData() { Name = "Количество", Value = this.Amount.ToString() },
                new ItemPointData() { Name = "Максимальное", Value = this._itemType.MaxAmount.ToString() },
                new ItemPointData() { Name = "Время роста", Value = this._itemType.growthTime.ToString() },
            });
        }
    }
}