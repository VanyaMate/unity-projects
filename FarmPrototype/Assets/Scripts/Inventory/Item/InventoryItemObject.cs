using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VM.UI;
using VM.Inventory.Items;
using VM.CameraTools;

namespace VM.Inventory
{
    public class InventoryItemObject : InteractableItem
    {
        [Header("Startup props")]
        [SerializeField] private SO_InventoryItem _itemType;
        [SerializeField] private float _amount;
        [SerializeField] private Transform _model;

        [Header("Meshes")]
        [SerializeField] protected MeshFilter _meshFilter;
        [SerializeField] protected MeshRenderer _meshRenderer;

        [Header("Manager")]
        [SerializeField] private InventoryItem _manager;

        [HideInInspector] public InventoryItem Manager => _manager;

        public Mesh mesh => this._meshFilter.sharedMesh;
        public Material material => this._meshRenderer.sharedMaterial;
        public Transform model => _model;

        private void Awake()
        {
            this.SetItemType(this._itemType, this._amount);
        }

        public void SetItemType (SO_InventoryItem itemType, float amount)
        {
            if (itemType != null)
            {
                this._itemType = itemType;
                this._amount = amount;

                if (this._manager != null)
                {
                    //Debug.Log("RemoveFromCommonManager");
                    this._manager.RemoveFromCommonManager();
                }

                switch (this._itemType.Type)
                {
                    case "Еда":
                        this._manager = new InventoryItem(this._itemType, this._amount, gameObject);
                        break;
                    case "Лопата":
                        this._manager = new InventoryItemShovel(this._itemType, this._amount, gameObject);
                        break;                    
                    case "Коса":
                        this._manager = new InventoryItemScythe(this._itemType, this._amount, gameObject);
                        break;
                    case "Постройка":
                        this._manager = new InventoryItemBuilding((SO_InventoryBuildingItem)this._itemType, this._amount, gameObject);
                        break;
                    case "Семена":
                        this._manager = new InventoryItemSeed((SO_InventorySeedsItem)this._itemType, this._amount, gameObject);
                        break;
                    case "Саженец":
                        this._manager = new InventoryItemSeedling((SO_InventorySeedlingsItem)this._itemType, this._amount, gameObject);
                        break;
                    case "Хранилище":
                        this._manager = new InventoryItemStorage((SO_InventoryStorageItem)this._itemType, this._amount, gameObject);
                        break;
                    case "Робот":
                        this._manager = new InventoryItemRobot((SO_InventoryRobotItem)this._itemType, this._amount, gameObject);
                        break;
                    case "Маяк":
                        this._manager = new InventoryItemPath((SO_InventoryPathItem)this._itemType, this._amount, gameObject);
                        break;
                    default:
                        this._manager = new InventoryItem(this._itemType, this._amount, gameObject);
                        break;
                }
            }
        }

        public void SetManager (InventoryItem manager)
        {
            if (this._manager != null)
            {
                this._manager.BeforeChangeManager();
                this._manager = null;
            }

            this._itemType = manager.Type;
            this._amount = manager.Amount;
            this._manager = manager;
        }

        public override void LeftClickAction()
        {
            this._manager.LeftClickGameObjectHandler();
        }

        public override void RightClickAction()
        {
            this._manager.RightClickGameObjectHandler();
        }

        public override void HoverAction()
        {
            this._manager.OnHoverGameObjectHandler();
        }
    }
}
