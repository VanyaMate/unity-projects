using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.InventoryManager
{
    [Serializable]
    public class InventoryItemManager
    {
        private SO_InventoryData _inventoryData;
        private float _amount;
        private Transform _itemOnScene;
        private InventoryManager _manager;

        public SO_InventoryData Data => _inventoryData;
        public float Amount
        {
            get => this._amount;
            set => this._amount = value;
        }
        public InventoryManager Manager => _manager;

        public UnityEvent<InventoryItemManager> OnItemChange = new UnityEvent<InventoryItemManager>();
        public UnityEvent<InventoryItemManager> OnItemDelete = new UnityEvent<InventoryItemManager>();

        public InventoryItemManager(SO_InventoryData inventoryData, float amount, InventoryManager manager = null)
        {
            if (inventoryData != null)
            {
                this._amount = amount > inventoryData.AmountMax ? inventoryData.AmountMax : amount;
            }

            this._inventoryData = inventoryData;
            this._manager = manager;

            this.OnItemChange.AddListener(
                (InventoryItemManager manager) => this.OnItemChangeHandler(manager)
            );
            this.OnItemDelete.AddListener(
                (InventoryItemManager manager) => this.DeleteFromScene(manager)
            );
        }

        public InventoryItemManager Get(float amount)
        {
            if (this._amount - amount >= 0)
            {
                this._amount -= amount;
                this.OnItemChange?.Invoke(this);
                return new InventoryItemManager(this.Data, amount);
            }
            else
            {
                return null;
            }
        }

        public bool Merge(InventoryItemManager mergedItem)
        {
            float delta = this.Data.AmountMax - this._amount;
            bool merged;

            if (mergedItem.Amount > delta)
            {
                this._amount = this.Data.AmountMax;
                mergedItem.Amount -= delta;
                merged = false;
            }
            else
            {
                this._amount += mergedItem.Amount;
                mergedItem.Amount = 0;
                merged = true;
            }

            this.OnItemChange?.Invoke(this);
            mergedItem.OnItemChange?.Invoke(mergedItem);

            return merged;
        }

        public void DeleteFromScene(InventoryItemManager item)
        {
            if (this._itemOnScene != null)
            {
                MonoBehaviour.Destroy(this._itemOnScene.gameObject);
                this._itemOnScene = null;
            }
        }

        public void SetItemOnScene(Transform itemOnScene)
        {
            this._itemOnScene = itemOnScene;
        }

        public void SetManager(InventoryManager manager)
        {
            this._manager = manager;
        }

        public void AddOnScene(Vector3 position)
        {
            this._itemOnScene = MonoBehaviour.Instantiate(this.Data.Prefab, position, Quaternion.identity);
            this._itemOnScene.TryGetComponent<InventoryItem>(out InventoryItem item);

            if (item != null)
            {
                item.SetManager(this);
            }
        }

        public void ResetData ()
        {
            this._inventoryData = null;
            this._amount = 0;
        }

        private void OnItemChangeHandler(InventoryItemManager item)
        {
            if (this.Amount <= 0)
            {
                this.OnItemDelete?.Invoke(this);
            }
        }
    }
}