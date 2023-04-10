using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;

namespace VM.Managers.Robots
{
    public class RobotPutTask : RobotTask
    {
        private RobotUnit _unit;
        private InventoryManager _storage;
        private float _amount;

        public RobotPutTask (RobotUnit unit, InventoryManager storage, float amount)
        {
            this._unit = unit;
            this._storage = storage;
            this._amount = amount;
        }

        public override void Action()
        {
            List<int> seedIds = this._unit.inventory.GetIndexSameType("Åäà");

            seedIds.ForEach((id) =>
            {
                InventoryItem item = this._unit.inventory.Get(id);
                if (!this._storage.Add(item))
                {
                    this._unit.inventory.AddToPosition(id, item);
                    this._storage.reservedSlots -= this._amount;
                    this.ended = true;
                }
            });

            this._storage.reservedSlots -= this._amount;
            this.ended = true;
        }

        public override void OnStart()
        {
            this.started = true;
        }
    }
}