using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers;
using VM.Managers.Save;
using VM.Save;
using VM.TerrainTools;

namespace VM.Player
{
    public class PlayerManager : ObjectToSave
    {
        public static PlayerManager Instance;

        [Header("Controller")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerController _playerController;

        [Header("Inventory")]
        [SerializeField] private InventoryManagerObject _pockets;
        [SerializeField] private InventoryPlayerHands _hands;

        [Header("Money")]
        [SerializeField] private float _startMoney = 0;
        [SerializeField] private MoneyManager _moneyManager;

        public MoneyManager moneyManager => _moneyManager;
        public InventoryManagerObject pockets => _pockets;
        public InventoryPlayerHands hands => _hands;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(this.CheckStayPosition());
            this._moneyManager = new MoneyManager(this._startMoney);
        }

        public IEnumerator CheckStayPosition ()
        {
            while (true)
            {
                yield return new WaitForSeconds(.1f);

                if (this._playerController.moved)
                {
                    RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, .2f);

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (
                            hits[i].transform.gameObject.layer == 7 && 
                            TerrainManager.Instance.PositionAcceptForChanges(transform.position, 2.5f)
                        )
                        {
                            TerrainManager.Instance.redactor.ChangeColorsFromTo(transform.position, 1.5f, 0, 3, .1f, .4f);
                            TerrainManager.Instance.redactor.RandomRemoveDetails(transform.position, 2f, 0, .15f);
                        }
                    }
                }
            }
        }

        public override string GetSaveData()
        {
            PlayerSaveData playerSaveData = new PlayerSaveData();
            InventoryManager playerInventory = InventoryPlayerPockets.Instance.Manager;
            InventoryManager playerHands = InventoryPlayerHands.instance.inventoryObject.Manager;

            playerSaveData.position = new SerVector(transform.position);
            playerSaveData.rotation = new SerQuaternion(transform.rotation);
            playerSaveData.money = this._moneyManager.money;
            playerSaveData.pockets = new InventoryManagerSaveData()
            {
                managerId = playerInventory.Type.Id,
                inventory = new Dictionary<int, InventoryItemSaveData>()
            };

            playerSaveData.hands = new InventoryManagerSaveData()
            {
                managerId = playerHands.Type.Id,
                inventory = new Dictionary<int, InventoryItemSaveData>()
            };

            for (int i = 0; i < playerInventory.Inventory.Count; i++)
            {
                InventoryItem item = playerInventory.Inventory[i];

                if (item != null)
                {
                    InventoryItemSaveData itemSaveData = item.GetSaveData();
                    playerSaveData.pockets.inventory.Add(i, itemSaveData);
                }
            }
            for (int i = 0; i < playerHands.Inventory.Count; i++)
            {
                InventoryItem item = playerHands.Inventory[i];

                if (item != null)
                {
                    InventoryItemSaveData itemSaveData = item.GetSaveData();
                    playerSaveData.hands.inventory.Add(i, itemSaveData);
                }
            }

            return JsonConvert.SerializeObject(playerSaveData);
        }

        public override void LoadSaveData(string data)
        {
            PlayerSaveData playerSaveData = JsonConvert.DeserializeObject<PlayerSaveData>(data);
            Vector3 translatePosition = new UnityVector3(playerSaveData.position).vector;
            Quaternion rotation = new UnityQuaternion(playerSaveData.rotation).quaternion;
            Vector3 currentPosition = this._characterController.transform.position;
            this._characterController.Move(translatePosition - currentPosition);
            this._characterController.transform.rotation = rotation;
            this._moneyManager.money = playerSaveData.money;

            InventoryPlayerPockets.Instance.ResetStorage();
            InventoryPlayerHands.instance.ResetStorage();

            this._LoadInventoryItem(playerSaveData.pockets, InventoryPlayerPockets.Instance.Manager);
            this._LoadInventoryItem(playerSaveData.hands, InventoryPlayerHands.instance.inventoryObject.Manager);
        }

        private void _LoadInventoryItem(InventoryManagerSaveData inventoryData, InventoryManager manager)
        {
            Dictionary<int, InventoryItemSaveData> inventory = inventoryData.inventory;

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
                    Debug.Log("is hranilishe");
                    this._LoadInventoryItem(
                        inventoryData: itemData.inventory,
                        manager: ((InventoryItemStorage)onScene.Manager).inventory
                    );
                }
                else if (inventoryItemType.Type == "Робот")
                {
                    Debug.Log("is robot");
                    this._LoadInventoryItem(
                        inventoryData: itemData.inventory,
                        manager: ((InventoryItemRobot)onScene.Manager).inventory
                    );  
                }
            }
        }
    }
}