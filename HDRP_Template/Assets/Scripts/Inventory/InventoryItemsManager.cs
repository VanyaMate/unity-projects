using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers;
using VM.Managers.Save;
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
                Quaternion quat = this._ghostItemQuat;
                Vector3 vquat = this._ghost.quat.eulerAngles;

                float x = size.x;
                float y = size.y;
                float z = size.z;

                if (Input.GetKey(KeyCode.Q))
                {
                    vquat = new Vector3(vquat.x, vquat.y + 1, vquat.z);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    vquat = new Vector3(vquat.x, vquat.y - 1, vquat.z);
                }

                float coef = vquat.x / 90;

                this._ghost.SetPosition(
                    Utils.MouseWorldPosition.point + new Vector3(0, y / 2, (-z / 2) * coef),
                    this._ghost.quat
                );
            }
        }

        public void FullReset()
        {
            this._items.ForEach((item) =>
            {
                item.RemoveFromScene(deleteFromCommonManager: false);
            });

            this._items.RemoveAll(x => true);
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
            Mesh mesh = item.Type.Prefab.mesh;
            Material material = item.Type.Prefab.material;
            Transform ghostModel = item.Type.Prefab.model;

            this._ghost.SetMesh(mesh);
            this._ghost.gameObject.SetActive(true);
            this._ghostItem = item;
            this._ghostItemSize = ghostModel.GetComponent<Renderer>().bounds.size;
            this._ghostItemQuat = this._ghost.quat = ghostModel.rotation;
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
                InventoryItemSaveData saveData = new InventoryItemSaveData()
                {
                    amount = item.Amount,
                    itemId = item.Type.Id,
                    position = new SerVector(item.OnScene.transform.position),
                    rotation = new SerQuaternion(item.OnScene.transform.rotation)
                };

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
                onScene.Manager.AddOnScene(
                    new UnityVector3(item.position).vector,
                    new UnityQuaternion(item.rotation).quaternion
                );
            });
        }
    }
}
