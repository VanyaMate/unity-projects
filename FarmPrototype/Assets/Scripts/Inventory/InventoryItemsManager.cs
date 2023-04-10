using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory.Items;
using VM.Managers;
using VM.Managers.Save;
using VM.Player;
using VM.Save;

namespace VM.Inventory
{
    public class InventoryItemsManager : ObjectToSave
    {
        public static InventoryItemsManager Instance = null;
        public static UnityEvent<List<InventoryItem>> OnInit = new UnityEvent<List<InventoryItem>>();

        [SerializeField] private Transform _container;
        [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();
        [SerializeField] private InventoryItemGhost _ghost;

        private InventoryItem _ghostItem;
        private Vector3 _ghostItemSize;
        private Quaternion _ghostItemQuat;

        public List<InventoryItem> Items => _items;
        public Transform Container => _container;
        public InventoryItemGhost ghost => _ghost;

        private void Awake()
        {
            Instance = this;
            OnInit.Invoke(this._items);
        }

        private void Update()
        {
            if (this._ghostItem != null)
            {
                Vector3 size = this._ghostItemSize;
                Quaternion quat = this._ghost.quat;
                Vector3 vquat = this._ghost.quat.eulerAngles;

                float x = size.x;
                float y = size.y;
                float z = size.z;

                if (Input.GetKey(KeyCode.Q))
                {
                    quat = Quaternion.AngleAxis(quat.eulerAngles.y - 1, Vector3.up);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    quat = Quaternion.AngleAxis(quat.eulerAngles.y + 1, Vector3.up);
                }

                float coef = vquat.x / 90;

                this._ghost.SetPosition(
                    Utils.MouseWorldPosition.point,
                    quat
                );
            }
        }

        public void FullReset()
        {
            this._items.ForEach((item) =>
            {
                item.RemoveFromScene(deleteFromCommonManager: false);
            });

            this._items.Clear();
            this._items = new List<InventoryItem>();
        }

        public InventoryItem Add(SO_InventoryItem itemType, float amount)
        {
            InventoryItem item = new InventoryItem(itemType, amount);
            this._items.Add(item);
            return item;
        }

        public void ShowGhost (InventoryItem item)
        {
            this._ghost.InsModel(item.Type.ClearPrefab);

            this._ghost.gameObject.SetActive(true);
            this._ghostItem = item;
            this._ghostItemQuat = Quaternion.FromToRotation(this._ghost.transform.position, PlayerManager.Instance.transform.position);
        }

        public void HideGhost ()
        {
            this._ghostItem = null;
            this._ghost.gameObject.SetActive(false);
        }

        public override string GetSaveData()
        {
            List<InventoryItemSaveData> data = new List<InventoryItemSaveData>();

            this._items.ForEach((item) =>
            {
                InventoryItemSaveData saveData = item.GetSaveData();
                data.Add(saveData);
            });

            return JsonConvert.SerializeObject(data);
        }

        public override void LoadSaveData(string data)
        {
            List<InventoryItemSaveData> items = JsonConvert.DeserializeObject<List<InventoryItemSaveData>>(data);
            
            items.ForEach((item) =>
            {
                SO_InventoryItem itemType = InventoryListOfTypes.Instance.GetItemById(item.itemId);

                InventoryItemObject onScene = Instantiate(
                    itemType.Prefab,
                    Vector3.zero,
                    Quaternion.identity
                );

                onScene.SetItemType(itemType, item.amount);

                if (item.position != null)
                {
                    onScene.Manager.AddOnScene(
                        new UnityVector3(item.position).vector,
                        new UnityQuaternion(item.rotation).quaternion
                    );
                }
                else
                {
                    onScene.Manager.RemoveFromScene();
                }

                if (itemType.Type == "Хранилище")
                {
                    this._LoadInventoryItem(
                        inventoryData: item,
                        manager: ((InventoryItemStorage)onScene.Manager).inventory
                    );
                }
            });
        }

        private void _LoadInventoryItem (InventoryItemSaveData inventoryData, InventoryManager manager)
        {
            Dictionary<int, InventoryItemSaveData> inventory = inventoryData.inventory.inventory;

            foreach (KeyValuePair<int, InventoryItemSaveData> pair in inventory)
            {
                InventoryItemSaveData itemData = pair.Value;
                SO_InventoryItem inventoryItemType = InventoryListOfTypes.Instance.GetItemById(itemData.itemId); 
                
                InventoryItemObject onScene = Instantiate(
                   inventoryItemType.Prefab,
                   Vector3.zero,
                   Quaternion.identity
                );


                onScene.SetItemType(inventoryItemType, itemData.amount);
                manager.AddToPosition(pair.Key, onScene.Manager);
                onScene.Manager.RemoveFromScene();

                if (inventoryItemType.Type == "Хранилище")
                {
                    this._LoadInventoryItem(
                        inventoryData: itemData,
                        manager: ((InventoryItemStorage)onScene.Manager).inventory
                    );
                }
                else if (inventoryItemType.Type == "Робот")
                {
                    this._LoadInventoryItem(
                        inventoryData: itemData,
                        manager: ((InventoryItemRobot)onScene.Manager).inventory
                    );
                }
            }
        }
    }
}
