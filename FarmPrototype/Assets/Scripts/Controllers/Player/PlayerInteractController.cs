using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;

namespace VM.Player
{
    public class PlayerInteractController : MonoBehaviour
    {
        // show info about element
        // show ghost area to take
        [SerializeField] private Transform _ghostToTake;
        [SerializeField] private InventoryManager _hands;

        private void Start()
        {
            this._hands = PlayerManager.Instance.hands.inventoryObject.Manager;
            this._hands.OnInventoryChange.AddListener(this._HandsInventoryChangeHandler);
        }

        private void _HandsInventoryChangeHandler(Dictionary<int, InventoryItem> inventory)
        {
            InventoryItem itemOnHands = inventory[0];

            if (itemOnHands != null)
            {
                if (itemOnHands.Type.Type == "Хранилище")
                {
                    
                }
            }
            else
            {
                // hide any ghost 
            }
        }
    }
}
