using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Managers.Path;
using VM.UI;
using VM.UI.Path;

namespace VM.Inventory.Items
{
    public class InventoryItemPath : InventoryItem
    {
        protected new SO_InventoryPathItem _itemType;
        private bool _active;
        private bool _placed;
        private PathNode _pathNode;

        public bool placed { get => _placed; set => _placed = value; }
        public PathNode pathNode
        {
            get
            {
                if (this._onScene != null)
                {
                    PathNode pathNode = this._onScene.GetComponent<PathNode>();
                    this._pathNode = pathNode;
                    return pathNode;
                }
                else
                {
                    return null;
                }
            }
        }

        public InventoryItemPath(SO_InventoryPathItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._itemType = type;
        }

        public override void LeftClickGameObjectHandler()
        {
            if (this._placed)
            {
                this._OpenPathNodeMenu();
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
                context.Add("take", () => {
                    // take
                    if (InventoryPlayerPockets.Instance.Manager.Add(this))
                    {
                        PathManager.instance.RemoveNode(this.pathNode);
                        this._placed = false;
                    }
                });

                UserInterface.Instance.ContextMenu.Show(context);
            }
            else
            {
                base.RightClickGameObjectHandler();
            }
        }

        private void _OpenPathNodeMenu ()
        {
            Debug.Log("patrhnode => " + this.pathNode);
            PathNodeMenu.instance.SetPathNode(this.pathNode);
            PathNodeMenu.instance.Show();
        }

        public override void Activate()
        {
            this._active = true;
            PathManager.instance.Enable(true);
        }

        public override void DeActivate()
        {
            this._active = false;
            PathManager.instance.Enable(false);
        }
    }
}