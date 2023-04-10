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
        [SerializeField] private List<Transform> _magnitPoints;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

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

                switch (this._itemType.Type)
                {
                    case "Еда":
                        this._manager = new InventoryItem(this._itemType, this._amount, gameObject);
                        break;
                    case "Лопата":
                        this._manager = new InventoryItemShovel(this._itemType, this._amount, gameObject);
                        break;                    
                    case "Постройка":
                        this._manager = new InventoryItemBuilding((SO_InventoryBuildingItem)this._itemType, this._amount, gameObject);
                        break;
                    default:
                        this._manager = new InventoryItem(this._itemType, this._amount, gameObject);
                        break;
                }
            }
        }

        public void SetManager (InventoryItem manager)
        {
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
            // CameraManager.Instance.viewTarget.position = transform.position;
        }
    }
}
