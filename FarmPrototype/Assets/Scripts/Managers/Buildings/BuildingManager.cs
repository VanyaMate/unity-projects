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
        [SerializeField] private GhostObject _ghost;
        [SerializeField] private Transform _container;

        [Header("Settings ghost")]
        [SerializeField] private int _angle = 30;
        [SerializeField] private float _distance = .25f;

        private InventoryItemBuilding _ghostItem;
        private Vector3 _startVector;
        private bool _active = false;
        private Vector3 _changedPosition = Vector3.zero;
        private bool _lockMouseMovement = false;
        private Vector3 _mouseWorldPosition = Vector3.zero;

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
                this._mouseWorldPosition = this._lockMouseMovement ? this._mouseWorldPosition : Utils.MouseWorldPosition.point;
                Vector3 tempQuat = this._ghost.quat.eulerAngles;

                if (Input.GetMouseButtonDown(0) && Utils.MouseOverGameObject)
                {
                    InventoryItemBuilding item = this._ghostItem.Storage.Get<InventoryItemBuilding>(this._ghostItem, 1);
                    item.PlaceOnScene(this._ghost.position, Quaternion.Euler(tempQuat));

                    if (this._ghostItem != null && this._ghostItem.Amount <= 0)
                    {
                        this._ghostItem.Storage.Get<InventoryItemBuilding>(this._ghostItem);
                        this.HideGhost(activateMenu: true);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    this.HideGhost();
                }

                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    this._lockMouseMovement = !this._lockMouseMovement;
                }

                /*if (Input.GetKey(KeyCode.LeftControl))
                {
                    this._buildingItems.ForEach((InventoryItemBuilding buildingItem) =>
                    {
                        buildingItem.OnScene.GetComponent<BuildingItemObject>()?.magnitPoints.ForEach((BuildingMagnitBox magnitBox) =>
                        {
                            magnitBox.gameObject.SetActive(true);
                        });
                    });

                    if (Utils.MouseWorldPosition.collider && Utils.MouseWorldPosition.collider.TryGetComponent(out BuildingMagnitBox magnitBox))
                    {
                        Vector3 size = magnitBox.parent.size;
                        Vector3 offsetSide = Vector3.zero;

                        if (magnitBox.type == BuildingMagnitType.Left)
                        {
                            offsetSide = -magnitBox.parent.transform.forward;
                        }
                        else if (magnitBox.type == BuildingMagnitType.Right)
                        {
                            offsetSide = magnitBox.parent.transform.forward;
                        }

                        this._ghost.SetPosition(
                            position: magnitBox.parent.transform.position + offsetSide * size.z,
                            quaternion: magnitBox.parent.transform.rotation
                        );
                    }
                }
                else */
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    tempQuat = this.GetShiftGhostPositionByHandlers(tempQuat, this._angle, this._distance);
                    Vector3 newPosition = this._mouseWorldPosition + this._changedPosition;
                    float deltaXPosition = newPosition.x % this._distance;
                    float deltaYPosition = newPosition.y % this._distance;
                    float deltaZPosition = newPosition.z % this._distance;
                    Vector3 shiftPosition = this._mouseWorldPosition = new Vector3(
                        newPosition.x - deltaXPosition,
                        newPosition.y - deltaYPosition,
                        newPosition.z - deltaZPosition
                    );

                    this._ghost.SetPosition(
                        position: shiftPosition,
                        quaternion: Quaternion.Euler(tempQuat)
                    );
                }
                else
                {
                    tempQuat = this.GetGhostPositionByHandlers(tempQuat);
                    this._ghost.SetPosition(
                        position: this._mouseWorldPosition + this._changedPosition,
                        quaternion: Quaternion.Euler(tempQuat)
                    );
                }

            }


/*            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                // Hide magnits

                this._buildingItems.ForEach((InventoryItemBuilding buildingItem) =>
                {
                    buildingItem.OnScene.GetComponent<BuildingItemObject>()?.magnitPoints.ForEach((BuildingMagnitBox magnitBox) =>
                    {
                        magnitBox.gameObject.SetActive(false);
                    });
                });
            }*/
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
            this._lockMouseMovement = false;
            this._ghostItem = item;
            this._ghost.gameObject.SetActive(true);
            this._ghost.transform.rotation = Quaternion.identity;
            this._ghost.InsModel(((SO_InventoryBuildingItem)item.Type).BuildingClearPrefab);
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

        private Vector3 GetGhostPositionByHandlers (Vector3 tempQuat)
        {           
            if (Input.GetKey(KeyCode.Q))
            {
                tempQuat -= Vector3.up;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                tempQuat += Vector3.up;
            }
            else if (Input.GetKey(KeyCode.R))
            {
                tempQuat += Vector3.right;
            }
            else if (Input.GetKey(KeyCode.F))
            {
                tempQuat -= Vector3.right;
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                tempQuat += Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.X))
            {
                tempQuat -= Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                tempQuat = Vector3.zero;
                this._changedPosition = Vector3.zero;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                this._changedPosition += Vector3.left * Time.deltaTime * 5;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                this._changedPosition += Vector3.right * Time.deltaTime * 5;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                this._changedPosition += Vector3.forward * Time.deltaTime * 5;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                this._changedPosition += Vector3.back * Time.deltaTime * 5;
            }
            else if (Input.GetKey(KeyCode.PageUp))
            {
                this._changedPosition += Vector3.up * Time.deltaTime * 5;
            }
            else if (Input.GetKey(KeyCode.PageDown))
            {
                this._changedPosition += Vector3.down * Time.deltaTime * 5;
            }

            return tempQuat;
        }

        private Vector3 GetShiftGhostPositionByHandlers (Vector3 tempQuat, int angle, float distance)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                tempQuat -= Vector3.up * angle;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                tempQuat += Vector3.up * angle;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                tempQuat += Vector3.right * angle;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                tempQuat -= Vector3.right * angle;
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                tempQuat += Vector3.forward * angle;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                tempQuat -= Vector3.forward * angle;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                // Reset
                tempQuat = Vector3.zero;
                this._changedPosition = Vector3.zero;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this._changedPosition += Vector3.left * distance;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this._changedPosition += Vector3.right * distance;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                this._changedPosition += Vector3.forward * distance;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                this._changedPosition += Vector3.back * distance;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }
            else if (Input.GetKeyDown(KeyCode.PageUp))
            {
                this._changedPosition += Vector3.up * distance;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }
            else if (Input.GetKeyDown(KeyCode.PageDown))
            {
                this._changedPosition += Vector3.down * distance;
                Debug.Log("this._changedPosition " + this._changedPosition);

            }

            return tempQuat;
        }

        public override string GetSaveData()
        {
            List<InventoryBuildingSaveData> dataToSave = new List<InventoryBuildingSaveData>();

            this._buildingItems.ForEach((item) =>
            {
                InventoryBuildingSaveData saveData = new InventoryBuildingSaveData()
                {
                    itemId = item.Type.Id,
                    position = new SerVector(item.OnScene.transform.position),
                    rotation = new SerQuaternion(item.OnScene.transform.rotation)
                };

                dataToSave.Add(saveData);
            });

            return JsonConvert.SerializeObject(dataToSave);
        }

        public override void LoadSaveData(string data)
        {
            List<InventoryBuildingSaveData> items = JsonConvert.DeserializeObject<List<InventoryBuildingSaveData>>(data);

            items.ForEach((item) =>
            {
                SO_InventoryBuildingItem itemType = (SO_InventoryBuildingItem)InventoryListOfTypes.Instance.GetItemById(item.itemId);
                InventoryItemBuilding building = new InventoryItemBuilding(itemType, 1);

                building.PlaceOnScene(
                    new UnityVector3(item.position).vector,
                    new UnityQuaternion(item.rotation).quaternion
                );
/*
                BuildingItemObject buildingOnScene = Instantiate(
                    itemType.BuildingPrefab,
                    Vector3.zero,
                    Quaternion.identity
                );

                buildingOnScene.SetItemType(itemType, 1);

                ((InventoryItemBuilding)buildingOnScene.Manager).PlaceOnScene(
                    new UnityVector3(item.position).vector, 
                    new UnityQuaternion(item.rotation).quaternion
                );*/
            });
        }
    }
}