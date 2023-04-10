using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Managers;
using VM.Seeds;
using VM.UI;
using VM.UI.WindowInfo;

namespace VM.Inventory.Items
{
    public class InventoryItemSeedling : InventoryItem
    {
        protected new SO_InventorySeedlingsItem _itemType;
        private float _growthTime;
        private bool _placed;
        private bool _ready;
        private bool _damaged;

        public bool placed { get => _placed; set => _placed = value; }
        public bool ready => _ready;
        public bool damaged => _damaged;
        public float progress => 100 / this._itemType.growthTime * this._growthTime;
        public SeedsItemObject seedsItemObject;
        public new SO_InventorySeedlingsItem Type => _itemType;

        public float growthTime { 
            get => _growthTime; 
            set {
                _growthTime = value;

                if (_growthTime > _itemType.growthTime)
                {
                    this._ready = true;
                }

                if (_growthTime > _itemType.growthTime * 5)
                {
                    this._damaged = true;
                }
            }
        }

        public InventoryItemSeedling(SO_InventoryItem type, float amount, GameObject onScene = null) : base(type, amount, onScene)
        {
            this._itemType = (SO_InventorySeedlingsItem)type;
            this._active = false;
            this._placed = false;
            this._ready = false;
            this._damaged = false;
            this._onScene = onScene;

            if (onScene)
            {
                onScene.TryGetComponent<SeedsItemObject>(out this.seedsItemObject);
            }
        }

        public override void LeftClickGameObjectHandler()
        {
            if (this._placed)
            {
                this.Take(Input.GetKey(KeyCode.LeftAlt));
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
                context.Add("take", () => this.Take(force: true));

                UserInterface.Instance.ContextMenu.Show(context);
            }
            else
            {
                base.RightClickGameObjectHandler();
            }
        }

        public override void AddOnScene(Vector3 position, Quaternion rotation, bool addoncm = true)
        {
            // если объект не создан - создать
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    this._itemType.Prefab.gameObject,
                    position,
                    Quaternion.identity
                );

                this._storage = null;
                this._onScene.GetComponent<InventoryItemObject>().SetManager(this);
                base._AddToCommonManager();
            }

            // переместить объект на позицию спауна
            this._onScene.transform.parent = InventoryItemsManager.Instance.Container;
            this._onScene.transform.position = position;
            this._onScene.transform.rotation = rotation;
        }

        public override void PlaceOnScene(Vector3 position, Quaternion rotation)
        {
            if (this._storage != null)
            {
                this._storage.Get<InventoryItemSeedling>(this);
                this._storage = null;
            }

            // если объект не создан - создать
            if (this._onScene == null && !this._placed)
            {
                this._placed = true;
                this._onScene = MonoBehaviour.Instantiate(
                    this._itemType.seedlingItemPrefab.gameObject,
                    position,
                    rotation
                );

                this._onScene.GetComponent<SeedsItemObject>().SetManager(this);
                this._AddToSeedlingManager();
                base._RemoveFromCommonManager();
            }

            // переместить объект на позицию спауна
            this._onScene.transform.parent = SeedlingsManager.instance.container;
            this._onScene.transform.position = position;
            this._onScene.transform.rotation = rotation;

            this._onScene.TryGetComponent<SeedsItemObject>(out this.seedsItemObject);
        }

        public void Take (bool force = false, bool addToHands = false)
        {
            if (this._ready)
            {
                if (this._damaged)
                {
                    Debug.Log("Damaged, delete");
                    this.RemoveSeedlingFromScene();
                }
                else
                {
                    Debug.Log("Not Damaged, take");
                    InventoryItem item = new InventoryItem(this._itemType.finalItem, 1);

                    if (Input.GetKey(KeyCode.LeftControl) || addToHands)
                    {
                        InventoryManager hands = InventoryPlayerHands.instance.inventoryObject.Manager;

                        if ((hands.Inventory[0] != null) && (hands.Inventory[0].Type.Type == "Хранилище"))
                        {
                            InventoryItemStorage handsItemStorage = ((InventoryItemStorage)hands.Inventory[0]);

                            if (handsItemStorage.inventory.Add(item))
                            {
                                this.RemoveSeedlingFromScene();
                            }
                        }
                        else if (InventoryPlayerHands.instance.inventoryObject.Manager.Add(item))
                        {
                            this.RemoveSeedlingFromScene();
                        }
                    }
                    else if (InventoryPlayerPockets.Instance.Manager.Add(item))
                    {
                        this.RemoveSeedlingFromScene();
                    }
                }
            }
            else if (force)
            {
                if (Input.GetKey(KeyCode.LeftControl) || addToHands)
                {
                    InventoryManager hands = InventoryPlayerHands.instance.inventoryObject.Manager;

                    if ((hands.Inventory[0] != null) && (hands.Inventory[0].Type.Type == "Хранилище"))
                    {
                        InventoryItemStorage handsItemStorage = ((InventoryItemStorage)hands.Inventory[0]);

                        if (handsItemStorage.inventory.Add(this))
                        {
                            this.RemoveSeedlingFromScene();
                        }
                    }
                    else if (InventoryPlayerHands.instance.inventoryObject.Manager.Add(this))
                    {
                        this.RemoveSeedlingFromScene();
                    }
                }
                else if (InventoryPlayerPockets.Instance.Manager.Add(this))
                {
                    this.RemoveSeedlingFromScene();
                }
            }
        }

        public void RemoveSeedlingFromScene (bool deleteFromCommonManager = true)
        {
            this._placed = false;
            this.seedsItemObject = null;

            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);

                this._onScene = null;
                this.OnDestoyPrefab.Invoke();

                if (deleteFromCommonManager)
                {
                    this._RemoveFromSeedlingManager();
                }

                base._RemoveFromCommonManager();
            }
        }

        public void AddToSeedlingManager ()
        {
            this._AddToSeedlingManager();
        }

        public override void Activate()
        {
            this._active = true;
            SeedlingsManager.instance.ShowGhost(this);
        }

        public override void DeActivate()
        {
            this._active = false;
            SeedlingsManager.instance.HideGhost();
        }

        protected override void _OpenInfo()
        {
            Window window = UserInterface.Instance.OpenWindow(this._itemType.Name);
            WindowItemInfo itemInfo = MonoBehaviour.Instantiate(
                UserInterface.Instance.ItemInfo,
                window.WindowContainer
            );

            itemInfo.SetData(this._itemType.Icon, this._itemType.Name, new List<ItemPointData>()
            {
                new ItemPointData() { Name = "Количество", Value = this.Amount.ToString() },
                new ItemPointData() { Name = "Максимальное", Value = this._itemType.MaxAmount.ToString() },
                new ItemPointData() { Name = "Время роста", Value = this._itemType.growthTime.ToString() },
                new ItemPointData() { Name = "Вырос на", Value = this.progress.ToString() + " %" },
            });
        }

        private void _AddToSeedlingManager()
        {
            if (SeedlingsManager.instance != null)
            {
                SeedlingsManager.instance.seedlings.Add(this);
                InventoryItemsManager.Instance.Items.Remove(this);
            }
            else
            {
                SeedlingsManager.OnInit.AddListener((items) => items.Add(this));
                InventoryItemsManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }

        private void _RemoveFromSeedlingManager()
        {
            if (SeedlingsManager.instance != null)
            {
                SeedlingsManager.instance.seedlings.Remove(this);
            }
            else
            {
                SeedlingsManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}