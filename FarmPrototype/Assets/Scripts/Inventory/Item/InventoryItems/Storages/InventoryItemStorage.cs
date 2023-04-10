using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Building;
using VM.Managers;
using VM.UI;

namespace VM.Inventory.Items
{
    public class InventoryItemStorage : InventoryItem
    {
        protected new SO_InventoryStorageItem _itemType;
        private bool _placed;
        private InventoryManager _inventory;

        public InventoryManager inventory => _inventory;

        public InventoryItemStorage(SO_InventoryStorageItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._itemType = type;
            this._placed = false;
            this._inventory = new InventoryManager(type.storageType);

            if (onScene)
            {
                this._onScene = onScene;
                this._placed = true;
            }
        }

        public override void AddOnScene(Vector3 position, Quaternion rotation, bool addoncm = true)
        {
            this._placed = true;
            base.AddOnScene(position, rotation);
        }

        public override void LeftClickGameObjectHandler()
        {
            this._inventory.LeftClickGameObjectHandler();
        }

        public override void RightClickGameObjectHandler()
        {
            if (this._placed)
            {
                Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

                context.Add("info", this._OpenInfo);
                context.Add("take", () => {
                    if (InventoryPlayerHands.instance.inventoryObject.Manager.Add(this))
                    {
                        this.RemoveBuildFromScene();
                    } 
                });

                UserInterface.Instance.ContextMenu.Show(context);
            }
            else
            {
                base.RightClickGameObjectHandler();
            }
        }

        public override void Activate()
        {
            this._active = true;
            StorageManager.instance.takeAreaGhostArea = true;
            //_InventoryStoragesManager.Instance.ShowGhost(this);
        }

        public override void DeActivate()
        {
            this._active = false;
            StorageManager.instance.takeAreaGhostArea = false;
            StorageManager.instance.takeAreaGhost.gameObject.SetActive(false);
            //_InventoryStoragesManager.Instance.HideGhost();
        }

        public void RemoveBuildFromScene(bool deleteFromCommonManager = true)
        {
            this._placed = false;

            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;

                if (deleteFromCommonManager)
                {
                    this._RemoveFromStorageManager();
                }
            }
        }

        private void _AddToStorageManager()
        {
            if (BuildingManager.instance != null)
            {
                WIPInventoryStoragesManager.Instance.Storages.Add(this);
            }
            else
            {
                WIPInventoryStoragesManager.OnInit.AddListener((items) => items.Add(this));
            }
        }

        private void _RemoveFromStorageManager()
        {
            if (BuildingManager.instance != null)
            { 
                WIPInventoryStoragesManager.Instance.Storages.Remove(this);
            }
            else
            {
                WIPInventoryStoragesManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}