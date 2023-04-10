using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;

namespace VM.Managers.Robots
{
    public class RobotTakeTask : RobotTask
    {
        private RobotUnit _unit;
        private InventoryManager _storage;
        private SO_InventoryItem _takeItem;
        private float _amount;

        public RobotTakeTask (RobotUnit unit, InventoryManager storage, SO_InventoryItem item, float amount = -1)
        {
            this._unit = unit;
            this._storage = storage;
            this._takeItem = item;
            this._amount = amount;
        }

        public override void Action()
        {
            float gettedItemsAmount = this._storage.Get(this._takeItem, this._amount);
            float casheAmount = gettedItemsAmount;

            while (gettedItemsAmount > 0)
            {
                float getAmount = gettedItemsAmount > this._takeItem.MaxAmount ? this._takeItem.MaxAmount : gettedItemsAmount;

                InventoryItemObject item = MonoBehaviour.Instantiate(
                    this._takeItem.Prefab,
                    Vector3.zero,
                    Quaternion.identity
                );

                item.SetItemType(this._takeItem, getAmount);
                InventoryItem itemManager = item.Manager;

                if (!((InventoryItemRobot)this._unit.robotOnScene.Manager).inventory.Add(itemManager))
                {
                    this._storage.Add(itemManager);
                }

                itemManager.RemoveFromScene();
                gettedItemsAmount -= getAmount;
            }

            this._amount -= casheAmount;

            if (casheAmount == 0 || this._amount == 0)
            {
                this.ended = true;
            }
        }

        public override void OnStart()
        {
            this.started = true;
        }
    }
}