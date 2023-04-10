using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Path;

namespace VM.Managers.Robots
{
    public class RobotSeedlingTask : RobotTask
    {
        private RobotUnit _unit;
        private PathNode _node;

        public RobotSeedlingTask (RobotUnit unit, PathNode node)
        {
            this._unit = unit;
            this._node = node;
        }

        public override void Action()
        {
            List<int> seedlingPlaces = this._unit.inventory.GetIndexSameType("Семена");
            bool hasSeedlingsInventory = seedlingPlaces.Count > 0;
            bool hasFreeSlots = this._node.unusabledSeedlingsPlace.Count > 0;

            if (hasSeedlingsInventory && hasFreeSlots)
            {
                if (this._node.CheckSeedlingPlace(this._node.unusabledSeedlingsPlace[0], out Vector3 point))
                {
                    InventoryItem gettedItem = this._unit.inventory.Get(seedlingPlaces[0], 1);
                    InventoryItemSeed item = new InventoryItemSeed((SO_InventorySeedsItem)gettedItem.Type, 1);
                    InventoryItemSeedling seedling = item.PlaceOnScene(
                        position: point,
                        rotation: Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0))
                    );

                    this._node.unusabledSeedlingsPlace.Remove(this._node.unusabledSeedlingsPlace[0]);
                    this._node.seedlingsItems.Add(seedling);
                }
            }
            else
            {
                this._node.reserved = false;
                this.ended = true;
            }
        }

        public override void OnStart()
        {
            this.started = true;
        }
    }
}