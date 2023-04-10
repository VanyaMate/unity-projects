using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Managers.Save;

namespace VM.Managers.Store
{
    public class StoreManager : ObjectToSave
    {
        public static StoreManager instance;

        private List<StoreManagerObject> _stores = new List<StoreManagerObject>();

        private void Awake()
        {
            instance = this;
        }

        public void AddStore (StoreManagerObject store)
        {
            if (this._stores.Find(x => x == store) == null)
            {
                this._stores.Add(store);
            }
        }

        public void RemoveStore (StoreManagerObject store)
        {
            this._stores.Remove(store);
        }

        public StoreManagerObject FindNearestStore (Vector3 position)
        {
            StoreManagerObject nearest = null;
            float distance = 10000;

            this._stores.ForEach((store) =>
            {
                float storeDistance = Vector3.Distance(store.transform.position, position);

                if (storeDistance < distance)
                {
                    nearest = store;
                    distance = storeDistance;
                }
            });

            return nearest;
        }

        public StoreManagerObject FindNearestStoreByItem(Vector3 position, SO_InventoryItem type)
        {
            StoreManagerObject nearest = null;
            float distance = 10000;

            this._stores.ForEach((store) =>
            {
                float storeDistance = Vector3.Distance(store.transform.position, position);

                if (storeDistance < distance && store.sellingItems.Find(x => !x.Removed && (x.Type == type)) != null)
                {
                    nearest = store;
                    distance = storeDistance;
                }
            });

            return nearest;
        }

        public bool CheckStoresWithItems (List<SO_InventoryItem> items)
        {
            return items.Find(
                (item) => this._stores.Find(
                    (store) => store.sellingItems.Find(
                        (sellingItem) => item == sellingItem.Type
                    ) != null
                ) != null
            ) != null;
        }

        public override string GetSaveData()
        {
            return "";
        }

        public override void LoadSaveData(string data)
        {

        }
    }
}