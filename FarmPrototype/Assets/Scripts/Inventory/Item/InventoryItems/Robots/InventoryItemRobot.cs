using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers.Robots;
using VM.UI;

namespace VM.Inventory.Items
{
    public class InventoryItemRobot : InventoryItem
    {
        private InventoryManager _inventory;

        public InventoryManager inventory => _inventory;

        public InventoryItemRobot(SO_InventoryRobotItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._inventory = new InventoryManager(type.robotInventoryType);

            if (onScene != null)
            {
                this._AddToRobotManager();
                this._RemoveFromCommonManager();
            }
        }

        public void SetInventoryManager (InventoryManager manager)
        {
            this._inventory.FullReset();
            this._inventory = manager;
        }

        public override void LeftClickGameObjectHandler()
        {

        }

        public override void RightClickGameObjectHandler()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);
            context.Add("take", () => {
                RobotUnit unit = this.OnScene.GetComponent<RobotUnit>();
                if (InventoryPlayerPockets.Instance.Manager.Add(this))
                {
                    RobotManager.instance.onSceneUnits.Remove(unit);
                    this._RemoveFromRobotManager();
                    base.RemoveFromScene();
                }
            });
            context.Add("init sys", () =>
            {
                if (this._onScene != null)
                {
                    Debug.Log("on scene");
                    RobotUnit unit = this.OnScene.GetComponent<RobotUnit>();
                    unit.Power(true);
                }

                this._AddToRobotManager();
            });
            context.Add("remove sys", () =>
            {
                this._RemoveFromRobotManager();
            });
            context.Add("open inv", () => this._OpenRobotInventory());

            UserInterface.Instance.ContextMenu.Show(context);
        }

        public override void AddOnScene(Vector3 position, Quaternion rotation, bool addtocm = true)
        {
            base.AddOnScene(position, rotation, false);
            this._AddToRobotManager();
        }

        public override void RemoveFromScene(bool deleteFromCommonManager = true)
        {
            if (this.OnScene != null)
            {
                RobotUnit robotUnit = this.OnScene.GetComponent<RobotUnit>();
                RobotManager.instance.onSceneUnits.Remove(robotUnit);
                RobotManager.instance.units.Remove(robotUnit);
            }

            base.RemoveFromScene(deleteFromCommonManager);
        }

        private void _OpenRobotInventory ()
        {
            this._inventory.OpenWindow();
        }

        private void _RemoveFromRobotManager ()
        {
            RobotUnit unit = this.OnScene.GetComponent<RobotUnit>();
            if (RobotManager.instance == null)
            {
                RobotManager.OnLoad.AddListener(() =>
                {
                    RobotManager.instance.units.Remove(unit);
                    unit.Power(false);
                });
            }
            else
            { 
                RobotManager.instance.units.Remove(unit);
                unit.Power(false);
            }
        }

        private void _AddToRobotManager ()
        {
            RobotUnit unit = this.OnScene.GetComponent<RobotUnit>();
            if (RobotManager.instance == null)
            {
                RobotManager.OnLoad.AddListener(() =>
                {
                    this._AddUnitToManager(unit);
                });
            }
            else
            {
                this._AddUnitToManager(unit);
            }
        }

        private void _AddUnitToManager (RobotUnit unit)
        {
            if (!RobotManager.instance.onSceneUnits.Contains(unit))
            {
                // Debug.Log("Add from - item robots");
                RobotManager.instance.onSceneUnits.Add(unit);
            }

            if (unit.inited && !RobotManager.instance.units.Contains(unit))
            {
                RobotManager.instance.units.Add(unit);
            }
        }
    }
}