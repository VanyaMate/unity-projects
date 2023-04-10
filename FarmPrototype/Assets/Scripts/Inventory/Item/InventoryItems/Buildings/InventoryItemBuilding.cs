using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Building;
using VM.Inventory;
using VM.UI;

namespace VM.Inventory.Items
{
    [Serializable]
    public class InventoryItemBuilding : InventoryItem
    {
        protected new SO_InventoryBuildingItem _itemType;
        private bool _placed;

        public InventoryItemBuilding(SO_InventoryBuildingItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._itemType = type;
            this._placed = false;
        }

        public override void LeftClickGameObjectHandler()
        {
            if (this._placed)
            {
                Debug.Log("click");
            }
            else
            {
                base.LeftClickGameObjectHandler();
            }
        }

        public override void RightClickGameObjectHandler()
        {
            if (this._placed)
            {
                Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

                context.Add("info", this._OpenInfo);
                context.Add("destroy", () => {
                    if (InventoryPlayerPockets.Instance.Manager.Add(this))
                    {
                        this.RemoveBuildFromScene();
                    }
                });

                UserInterface.Instance.ContextMenu.Show(context);
            }
            else
            {
                base.RightClickGameObjectHandler();
            }
        }

        public override void Activate()
        {
            this._active = true;
            BuildingManager.instance.ShowGhost(this);
        }

        public override void DeActivate()
        {
            this._active = false;
            BuildingManager.instance.HideGhost();
        }

        public override void PlaceOnScene (Vector3 position, Quaternion rotation)
        {
            Debug.Log("PlaceOnScene - Building");
            this._placed = true;

            Debug.Log(this._onScene);

            // если объект не создан - создать
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    ((SO_InventoryBuildingItem)this._itemType).BuildingPrefab.gameObject,
                    position,
                    rotation
                );

                this._storage = null;
                this._onScene.GetComponent<BuildingItemObject>().SetManager(this);
                Debug.Log("PlaceOnScene - Execute");
                this._AddToBuildingManager();
            }

            // переместить объект на позицию спауна
            this._onScene.transform.parent = InventoryItemsManager.Instance.Container;
            this._onScene.transform.position = position;
            this._onScene.transform.rotation = rotation;
        }

        public void RemoveBuildFromScene(bool deleteFromCommonManager = true)
        {
            this._placed = false;

            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;

                if (deleteFromCommonManager)
                {
                    this._RemoveFromBuildingManager();
                }
            }
        }

        private void _AddToBuildingManager()
        {
            Debug.Log("Add To Building Manager");

            if (BuildingManager.instance != null)
            {
                BuildingManager.instance.Items.Add(this);
            }
            else
            {
                BuildingManager.OnInit.AddListener((items) => items.Add(this));
            }
        }

        private void _RemoveFromBuildingManager()
        {
            if (BuildingManager.instance != null)
            {
                BuildingManager.instance.Items.Remove(this);
            }
            else
            {
                BuildingManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}