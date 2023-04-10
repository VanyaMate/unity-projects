using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Path;

namespace VM.Managers.Robots
{
    public class RobotTakeSeedlingTask : RobotTask
    {
        private RobotUnit _unit;
        private PathNode _node;

        public RobotTakeSeedlingTask (RobotUnit unit, PathNode node)
        {
            this._unit = unit;
            this._node = node;
        }

        public override void Action()
        {
            InventoryItemSeedling readySeedling = this._node.seedlingsItems.Find((seedling) => seedling.ready);

            if (readySeedling != null)
            {
                if (readySeedling.damaged == true)
                {
                    this._RemoveSeedlingNode(readySeedling);
                    return;
                }

                if (readySeedling.OnScene == null)
                {
                    return;
                }

                InventoryItem seedItem = new InventoryItem(readySeedling.Type.finalItem, 1);

                if (this._unit.inventory.Add(seedItem))
                {
                    this._RemoveSeedlingNode(readySeedling);
                }
                else
                {
                    this._node.reserved = false;
                    this.ended = true;
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

        private void _RemoveSeedlingNode (InventoryItemSeedling readySeedling)
        {
            Vector3 seedlingPosition = readySeedling.OnScene.transform.position;

            readySeedling.OnScene.layer = 2;
            readySeedling.RemoveSeedlingFromScene();
            this._node.seedlingsItems.Remove(readySeedling);

            if (this._node.CheckSeedlingPlace(seedlingPosition, out Vector3 point, true))
            {
                this._node.unusabledSeedlingsPlace.Add(seedlingPosition);
            }
        }
    }
}
