using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory.Items;
using VM.Managers.Save;
using VM.Save;

namespace VM.Inventory
{
    public class WIPInventoryStoragesManager : ObjectToSave
    {
        public static WIPInventoryStoragesManager Instance = null;
        public static UnityEvent<List<InventoryItemStorage>> OnInit = new UnityEvent<List<InventoryItemStorage>>();

        [SerializeField] private Transform _container;
        [SerializeField] private List<InventoryItemStorage> _storages = new List<InventoryItemStorage>();
        [SerializeField] private InventoryItemGhost _ghost;

        private InventoryItem _ghostItem;
        private Vector3 _ghostItemSize;
        private Quaternion _ghostItemQuat;

        public List<InventoryItemStorage> Storages => _storages;
        public Transform Container => _container;
        public InventoryItemGhost ghost => _ghost;

        private void Awake()
        {
            Instance = this;
            OnInit.Invoke(this._storages);
        }

        public void FullReset()
        {
            this._storages.ForEach((storage) =>
            {
                //storage.FullReset();
                storage.RemoveFromScene();
            });

            this._storages = new List<InventoryItemStorage>();
        }

        public InventoryItemStorage Add(SO_InventoryStorageItem inventoryType)
        {
            InventoryItemStorage manager = new InventoryItemStorage(inventoryType, 1);
            this._storages.Add(manager);
            return manager;
        }

        public override string GetSaveData()
        {
            List<InventoryManagerSaveData> data = new List<InventoryManagerSaveData>();

            this._storages.ForEach((storage) =>
            {
                InventoryManagerSaveData saveData = new InventoryManagerSaveData()
                {
                    managerId = storage.Type.Id,
                    inventory = new Dictionary<int, InventoryItemSaveData>(),
                    position = new SerVector(storage.OnScene.transform.position),
                    rotation = new SerQuaternion(storage.OnScene.transform.rotation)
                };

               /* for (int i = 0; i < storage.Inventory.Count; i++)
                {
                    InventoryItem item = storage.Inventory[i];

                    if (item != null)
                    {
                        saveData.inventory.Add(i, new InventoryItemSaveData()
                        {
                            amount = item.Amount,
                            itemId = item.Type.Id
                        });
                    }
                }*/

                data.Add(saveData);
            });

            return JsonConvert.SerializeObject(data);
        }

        public override void LoadSaveData(string data)
        {
            List<InventoryManagerSaveData> storagesData = JsonConvert.DeserializeObject<List<InventoryManagerSaveData>>(data);

            storagesData.ForEach((storage) =>
            {
                /*SO_InventoryManager storageType = InventoryListOfTypes.Instance.GetStorageById(storage.managerId);
                InventoryItemStorage manager = new InventoryItemStorage(storageType);
                manager.AddOnScene(
                    new UnityVector3(storage.position).vector,
                    new UnityQuaternion(storage.rotation).quaternion
                );

                Dictionary<int, InventoryItemSaveData> inventory = storage.inventory;

                foreach (KeyValuePair<int, InventoryItemSaveData> pair in inventory)
                {
                    InventoryItemSaveData itemData = pair.Value;
                    SO_InventoryItem itemType = InventoryListOfTypes.Instance.GetItemById(itemData.itemId);
                    InventoryItemStorage item = new InventoryItemStorage(itemType, itemData.amount);
                    manager.AddToPosition(pair.Key, item);
                }*/
            });
        }
    }
}