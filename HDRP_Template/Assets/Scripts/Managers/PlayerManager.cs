using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Managers.Save;
using VM.Save;

namespace VM.Player
{
    public class PlayerManager : ObjectToSave
    {
        public static PlayerManager Instance;

        [Header("Controller")]
        [SerializeField] private CharacterController _characterController;

        [Header("Inventory")]
        [SerializeField] private InventoryManagerObject _pockets;
        [SerializeField] private InventoryPlayerHands _hands;

        private void Awake()
        {
            Instance = this;
        }

        public override string GetSaveData()
        {
            PlayerSaveData playerSaveData = new PlayerSaveData();
            InventoryManager playerInventory = InventoryPlayerPockets.Instance.Manager;

            playerSaveData.position = new SerVector(transform.position);
            playerSaveData.rotation = new SerQuaternion(transform.rotation);
            playerSaveData.pockets = new InventoryManagerSaveData()
            {
                managerId = playerInventory.Type.Id,
                inventory = new Dictionary<int, InventoryItemSaveData>()
            };

            for (int i = 0; i < playerInventory.Inventory.Count; i++)
            {
                InventoryItem item = playerInventory.Inventory[i];

                if (item != null)
                {
                    InventoryItemSaveData itemSaveData = new InventoryItemSaveData()
                    {
                        amount = item.Amount,
                        itemId = item.Type.Id
                    };

                    playerSaveData.pockets.inventory.Add(i, itemSaveData);
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

            InventoryPlayerPockets.Instance.ResetStorage();

            foreach (KeyValuePair<int, InventoryItemSaveData> pair in playerSaveData.pockets.inventory)
            {
                InventoryItemSaveData itemData = pair.Value;
                SO_InventoryItem itemType = InventoryListOfTypes.Instance.GetItemById(itemData.itemId);
                InventoryItem item = new InventoryItem(itemType, itemData.amount);
                InventoryPlayerPockets.Instance.Manager.AddToPosition(pair.Key, item);
            }
        }
    }
}