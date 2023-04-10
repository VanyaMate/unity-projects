using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers;
using VM.Managers.Save;
using VM.Save;
using VM.UI;

namespace VM.Building
{
    public class BuildingManager : ObjectToSave
    {
        public static BuildingManager instance; 
        public static UnityEvent<List<InventoryItemBuilding>> OnInit = new UnityEvent<List<InventoryItemBuilding>>();

        [SerializeField] private List<InventoryItemBuilding> _buildingItems = new List<InventoryItemBuilding>();
        [SerializeField] private BuildingItemGhost _ghost;
        [SerializeField] private Transform _container;

        private InventoryItemBuilding _ghostItem;
        private Vector3 _startVector;
        private bool _active = false;

        public List<InventoryItemBuilding> Items => _buildingItems;
        public Transform container => _container;

        private void Awake()
        {
            instance = this;
            OnInit.Invoke(this._buildingItems);
            this.HideGhost();
        }
        
        private void Update()
        {
            if (this._ghost.gameObject.activeSelf)
            {
                Quaternion tempQuat = this._ghost.quat;

                if (Input.GetMouseButtonDown(0) && Utils.MouseOverGameObject)
                {
                    InventoryItemBuilding item = this._ghostItem.Storage.Get<InventoryItemBuilding>(this._ghostItem, 1);
                    item.PlaceOnScene(this._ghost.position, this._ghost.transform.rotation);

                    if (this._ghostItem.Amount <= 0)
                    {
                        this._ghostItem.Storage.Get<InventoryItemBuilding>(this._ghostItem);
                        this.HideGhost(activateMenu: true);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    this.HideGhost();
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    tempQuat = Quaternion.AngleAxis(tempQuat.eulerAngles.y - 1, Vector3.up);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    tempQuat = Quaternion.AngleAxis(tempQuat.eulerAngles.y + 1, Vector3.up);
                }

                this._ghost.SetPosition(Utils.MouseWorldPosition.point, tempQuat);
            }
        }

        public void FullReset ()
        {
            this._buildingItems.ForEach((item) =>
            {
                item.RemoveFromScene(deleteFromCommonManager: false);
            });

            this._buildingItems.RemoveAll(x => true);
            this._buildingItems = new List<InventoryItemBuilding>();
        }

        public void ShowGhost (InventoryItemBuilding item)
        {
            MenuController.blockOpenMenu = true;
            this._active = true;
            this._ghostItem = item;
            this._ghost.gameObject.SetActive(true);
            this._ghost.SetMesh(((SO_InventoryBuildingItem)this._ghostItem.Type).BuildingPrefab.mesh);
        }

        public void HideGhost (bool activateMenu = false)
        {
            if (activateMenu)
            {
                MenuController.blockOpenMenu = true;
            }

            this._active = false;
            this._ghostItem = null;
            this._ghost.gameObject.SetActive(false);
        }

        public override string GetSaveData()
        {
            List<InventoryBuildingSaveData> data = new List<InventoryBuildingSaveData>();

            this._buildingItems.ForEach((item) =>
            {
                InventoryBuildingSaveData saveData = new InventoryBuildingSaveData()
                {
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
            List<InventoryBuildingSaveData> items = JsonConvert.DeserializeObject<List<InventoryBuildingSaveData>>(data);

            items.ForEach((item) =>
            {
                SO_InventoryBuildingItem itemType = (SO_InventoryBuildingItem)InventoryListOfTypes.Instance.GetItemById(item.itemId);

                BuildingItemObject onScene = Instantiate(
                    itemType.BuildingPrefab,
                    Vector3.zero,
                    Quaternion.identity
                );

                onScene.SetItemType(itemType, 1);
                ((InventoryItemBuilding)onScene.Manager).PlaceOnScene(
                    new UnityVector3(item.position).vector, 
                    new UnityQuaternion(item.rotation).quaternion
                );
            });
        }
    }
}