using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.Inventory
{
    public class InventoryPlayerHands : MonoBehaviour
    {
        public static InventoryPlayerHands instance;

        [SerializeField] private InventoryManagerObject _handsInventory;
        [SerializeField] private InventoryItemHand _handsPosition;

        private UnityAction _deactivateItem;
        private GameObject _itemInHands;

        public InventoryManagerObject inventoryObject => _handsInventory;
        public InventoryItemHand handsPosition => _handsPosition;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            this._handsInventory.Manager.OnInventoryChange.AddListener((Dictionary<int, InventoryItem> inventory) =>
            {
                this._DeActivate();
                this.Activate();
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this._DeActivate();
            }
        }

        public void AddToHands(InventoryItem item)
        {
            this.RemoveFromHands();

            GameObject itemModel = item.Type.ClearPrefab;
            // Mesh itemMesh = itemModel.GetComponent<MeshFilter>().sharedMesh;
            // Material itemMateral = itemModel.GetComponent<MeshRenderer>().sharedMaterial;

            // this._handsPosition.meshFilter.mesh = itemMesh;
            // this._handsPosition.meshRenderer.material = itemMateral;
            this._itemInHands = Instantiate(itemModel, this._handsPosition.transform);


            this._handsPosition.gameObject.SetActive(true);
        }

        public void RemoveFromHands ()
        {
            Destroy(this._itemInHands);
            this._handsPosition.gameObject.SetActive(false);
        }

        public void ResetStorage()
        {
            for (int i = 0; i < this._handsInventory.Manager.Inventory.Count; i++)
            {
                this._handsInventory.Manager.Get(i);
            }
        }

        public void Activate ()
        {
            Dictionary<int, InventoryItem> inventory = this._handsInventory.Manager.Inventory;
            InventoryItem item = inventory[0];

            if (item != null)
            {
                this.AddToHands(item);
                item.Activate();
                this._deactivateItem = item.DeActivate;
            }
            else
            {
                this._deactivateItem = null;
            }
        }

        private void _DeActivate ()
        {
            if (this._deactivateItem != null)
            {
                this.RemoveFromHands();
                this._deactivateItem();
            }
        }
    }
}