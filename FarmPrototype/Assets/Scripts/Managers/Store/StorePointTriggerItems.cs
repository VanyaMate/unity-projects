using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Inventory.Items;

namespace VM.Managers.Store
{
    public class StorePointTriggerItems : MonoBehaviour
    {
        [SerializeField] private float _checkRate;
        [SerializeField] private BoxCollider _collider;

        private float _currentTimeCheck = 0;
        private bool _saveItems = true;

        public UnityEvent<List<InventoryItem>> OnItemsUpdate = new UnityEvent<List<InventoryItem>>();
        public List<InventoryItem> items = new List<InventoryItem>();

        private void Awake()
        {
            StartCoroutine(this._UpdateItems());
        }

        public void UpdateInfo ()
        {
            Collider[] buffer = new Collider[256];
            this.items = new List<InventoryItem>();

            int colliders = Physics.OverlapBoxNonAlloc(transform.position + this._collider.center, this._collider.size, buffer, transform.rotation);

            for (int i = 0; i < colliders; i++)
            {
                // if this item is inventoryItem
                if (buffer[i].TryGetComponent<InventoryItemObject>(out InventoryItemObject item))
                {
                    if (!item.Manager.Removed && item.Manager.Type.AvailInStore && item.Manager.Type.Type != "Хранилище")
                    {
                        this.items.Add(item.Manager);
                    }
                    else if (item.Manager.Type.Type == "Хранилище")
                    {
                        Dictionary<int, InventoryItem> inventory = ((InventoryItemStorage)item.Manager).inventory.Inventory;

                        foreach (KeyValuePair<int, InventoryItem> pair in inventory)
                        {
                            if (pair.Value != null && pair.Value.Type != null)
                            {
                                if (!item.Manager.Removed && item.Manager.Type.AvailInStore)
                                {
                                    this.items.Add(pair.Value);
                                }
                            }
                        }
                    }
                }
            }

            this.OnItemsUpdate.Invoke(this.items);
        }

        private IEnumerator _UpdateItems ()
        {
            while (true)
            {
                this.UpdateInfo();
                yield return new WaitForSeconds(this._checkRate);
            }
        }
    }
}