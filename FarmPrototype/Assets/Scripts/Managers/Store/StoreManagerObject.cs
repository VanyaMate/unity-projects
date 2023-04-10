using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Player;

namespace VM.Managers.Store
{
    public class StoreManagerObject : InteractableItem
    {
        [SerializeField] private StorePointTriggerItems _pointTrigger;

        private List<InventoryItem> _sellingItems = new List<InventoryItem>();

        public StorePointTriggerItems trigger => _pointTrigger;
        public List<InventoryItem> sellingItems => _sellingItems;

        private void Awake()
        {

        }

        private void Start ()
        {
            StoreManager.instance.AddStore(this);
            this._sellingItems = this._pointTrigger.items;
            this._pointTrigger.OnItemsUpdate.AddListener((List<InventoryItem> items) =>
            {
                items.ForEach((item) =>
                {
                    this._sellingItems = items;
                });
            });
        }

        public float PurchaseItems (SO_InventoryItem itemType, float amount = 1)
        {
            float purchases = 0;

            for (int i = 0; i < this._sellingItems.Count; i++)
            {
                InventoryItem item = this._sellingItems[i];

                if (item.Type == itemType)
                {
                    purchases += item.GetForce(amount);

                    if (purchases == amount)
                    {
                        break;
                    }
                }
            };

            PlayerManager.Instance.moneyManager.money += itemType.Cost * purchases;
            return purchases;
        }

        public override void HoverAction()
        {
        }

        public override void LeftClickAction()
        {
            this._sellingItems.ForEach((item) =>
            {
                // this.PurchaseItems(item);
            });
        }

        public override void RightClickAction()
        {
            Debug.Log("OpenContext");
        }
    }
}