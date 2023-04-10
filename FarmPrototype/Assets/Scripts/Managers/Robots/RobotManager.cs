using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Path;
using VM.Managers.Save;
using VM.Save;

namespace VM.Managers.Robots
{
    public class RobotManager : ObjectToSave
    {
        public static RobotManager instance;
        public static UnityEvent OnLoad = new UnityEvent();

        [SerializeField] private List<RobotUnit> _units = new List<RobotUnit>();
        [SerializeField] private List<RobotUnit> _allOnSceneUnits = new List<RobotUnit>();
        [SerializeField] private RobotAutotaskManager _autotaskManager = new RobotAutotaskManager();

        public List<RobotUnit> units => _units;
        public List<RobotUnit> onSceneUnits => _allOnSceneUnits;

        private void Awake()
        {
            instance = this;
            OnLoad.Invoke();

            StartCoroutine(this._AutotaskManager());
        }

        private void Update()
        {
            this._units.ForEach((unit) =>
            {
                unit.Action();
            });
        }

        public RobotUnit GetFreeRobot()
        {
            return this._units.Find((robot) => (robot.inited == true) && (robot.worked == false));
        }

        public List<RobotUnit> GetFreeRobots ()
        {
            return this._units.FindAll((robot) => (robot.inited == true) && (robot.worked == false));
        }

        public void FullReset ()
        {
            this._allOnSceneUnits.ForEach((unit) =>
            {
                unit.FullReset();
                Destroy(unit.gameObject);
            });

            this._allOnSceneUnits.Clear();
            this._units.Clear();
        }

        public override string GetSaveData()
        {
            List<RobotUnitSaveData> unitsSaveData = new List<RobotUnitSaveData>();
            // save units
            // save positions
            // save inventory
            // save tasks

            Debug.Log("SaveRobots: " + this._allOnSceneUnits.Count);

            this._allOnSceneUnits.ForEach((unit) =>
            {
                InventoryItemRobot itemRobot = (InventoryItemRobot)unit.robotOnScene.Manager;
                RobotUnitSaveData unitSaveData = new RobotUnitSaveData()
                {
                    robotId = unit.robotType.Id,
                    position = new SerVector(unit.transform.position),
                    rotation = new SerQuaternion(unit.transform.rotation),
                    inited = unit.inited,
                    inventoryManager = new InventoryManagerSaveData()
                };

                InventoryManagerSaveData storageSaveData = new InventoryManagerSaveData()
                {
                    managerId = itemRobot.inventory.Type.Id,
                    inventory = new Dictionary<int, InventoryItemSaveData>()
                };

                unitSaveData.inventoryManager = storageSaveData;

                for (int i = 0; i < itemRobot.inventory.Inventory.Count; i++)
                {
                    InventoryItem inventoryItem = itemRobot.inventory.Inventory[i];

                    if (inventoryItem != null)
                    {
                        storageSaveData.inventory.Add(i, inventoryItem.GetSaveData());
                    }
                }

                unitsSaveData.Add(unitSaveData);
            });

            return JsonConvert.SerializeObject(unitsSaveData);
        }

        public override void LoadSaveData(string data)
        {
            List<RobotUnitSaveData> unitsSaveData = JsonConvert.DeserializeObject<List<RobotUnitSaveData>>(data);

            Debug.Log("LoadRobots: " + unitsSaveData.Count);

            unitsSaveData.ForEach((unitSaveData) =>
            {
                SO_InventoryRobotItem robotType = (SO_InventoryRobotItem)InventoryListOfTypes.Instance.GetItemById(unitSaveData.robotId);

                InventoryItemObject robotObject = Instantiate(robotType.Prefab, transform);
                RobotUnit robotManager = robotObject.GetComponent<RobotUnit>();
                InventoryItemRobot robotItem = (InventoryItemRobot)robotObject.Manager;

                SO_InventoryManager inventoryManagerType = InventoryListOfTypes.Instance.GetStorageById(unitSaveData.inventoryManager.managerId);
                robotItem.SetInventoryManager(new InventoryManager(inventoryManagerType, robotObject.gameObject));
                
                foreach (KeyValuePair<int, InventoryItemSaveData> pair in unitSaveData.inventoryManager.inventory)
                {
                    SO_InventoryItem itemType = InventoryListOfTypes.Instance.GetItemById(pair.Value.itemId);

                    InventoryItemObject onScene = Instantiate(
                        itemType.Prefab,
                        Vector3.zero,
                        Quaternion.identity
                    );

                    onScene.SetItemType(itemType, pair.Value.amount);
                    robotItem.inventory.AddToPosition(pair.Key, onScene.Manager);
                    onScene.Manager.RemoveFromScene();

                    if (itemType.Type == "Хранилище")
                    {
                        Debug.Log("Хранилище в роботе");
                        this._LoadInventoryItem(
                            inventoryData: pair.Value,
                            manager: ((InventoryItemStorage)onScene.Manager).inventory
                        );
                    }
                    else if (itemType.Type == "Робот")
                    {
                        this._LoadInventoryItem(
                            inventoryData: pair.Value,
                            manager: ((InventoryItemRobot)onScene.Manager).inventory
                        );
                    }
                }

                robotManager.transform.position = new UnityVector3(unitSaveData.position).vector;
                robotManager.transform.rotation = new UnityQuaternion(unitSaveData.rotation).quaternion;
                robotManager.Power(unitSaveData.inited);

                if (!RobotManager.instance.onSceneUnits.Contains(robotManager))
                {
                    RobotManager.instance.onSceneUnits.Add(robotManager);
                }

                if (unitSaveData.inited)
                {
                    RobotManager.instance.units.Add(robotManager);
                }
            });
        }

        private void _LoadInventoryItem(InventoryItemSaveData inventoryData, InventoryManager manager)
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
    
        private IEnumerator _AutotaskManager ()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);

                this._autotaskManager.OnUpdate();
            }
        }
    }
}