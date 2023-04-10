using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Path;

namespace VM.Managers.Robots
{
    public class RobotUnit : MonoBehaviour
    {
        [Header("Props")]
        [SerializeField] private SO_InventoryRobotItem _robotType;

        [Header("Status")]
        [SerializeField] private bool _worked = false;
        [SerializeField] private bool _inited = false;
        [SerializeField] private List<RobotTask> _tasks = new List<Robots.RobotTask>();
        [SerializeField] private InventoryItemObject _robotOnScene;

        public float speed => this._robotType.speed;
        public bool worked => _worked;
        public bool inited => _inited;
        public SO_InventoryRobotItem robotType => _robotType;
        public InventoryItemObject robotOnScene => _robotOnScene;
        public InventoryManager inventory => ((InventoryItemRobot)this.robotOnScene.Manager).inventory;

        private void Awake()
        {
            TryGetComponent<InventoryItemObject>(out this._robotOnScene);
        }

        public void Power (bool value)
        {
            this._inited = value;
        }

        public void FullReset ()
        {
            this._inited = false;
            this._tasks.Clear();
        }

        public void Action ()
        {
            if (this._tasks.Count != 0)
            {
                this._worked = true;
                RobotTask task = this._tasks[0];
                task.Action();

                if (task.ended)
                {
                    this._tasks.Remove(task);

                    if (this._tasks.Count == 0)
                    {
                        this._worked = false;
                    }
                }
            }
        }

        public void AddTask (RobotTask task)
        {
            this._tasks.Add(task);
        }
    }
}