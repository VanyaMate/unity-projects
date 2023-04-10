using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory.Items;
using VM.Save;
using VM.UI;
using VM.UI.WindowInfo;

namespace VM.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] protected SO_InventoryItem _itemType;
        [SerializeField] protected GameObject _onScene;
        [SerializeField] private float _amount;

        protected bool _active;
        protected InventoryManager _storage;

        public UnityEvent<float> OnAmountChange = new UnityEvent<float>();
        public UnityEvent OnDelete = new UnityEvent();
        public UnityEvent OnDestoyPrefab = new UnityEvent();

        public SO_InventoryItem Type => _itemType;
        public float Amount
        {
            get => this._amount;
            set {
                this._amount = value;
                this.OnAmountChange.Invoke(this._amount);
            }
        }
        public InventoryManager Storage => _storage;
        public GameObject OnScene => _onScene;
        public bool Removed = false;
        public bool active => _active;

        public InventoryItem (SO_InventoryItem type, float amount, GameObject onScene = null)
        {
            this._itemType = type;
            this._amount = amount;
            this._onScene = onScene;

            if (this._onScene != null)
            {
                this._AddToCommonManager();
            }

            this.OnAmountChange.AddListener((float amount) => this._OnAmountChange());
        }

        public bool MergeWith (InventoryItem item, out float amount)
        {
            float sum = this._amount + item.Amount;

            if (sum > this._itemType.MaxAmount)
            {
                amount = sum - this._itemType.MaxAmount;
                this.Amount = this._itemType.MaxAmount;
                return false;
            }
            else
            {
                amount = 0;
                this.Amount += item.Amount;
                return true;
            }
        }

        public bool Get (float amount)
        {
            if (amount > this._amount)
            {
                return false;
            }
            else
            {
                this.Amount -= amount;
                return true;
            }
        }

        public float GetForce (float amount)
        {
            float getted = 0;

            if ((amount > this._amount) || (amount == -1))
            {
                getted = this._amount;
                this.Amount -= this._amount;
            }
            else
            {
                getted = amount;
                this.Amount -= amount;
            }

            return getted;
        }
        
        public void SetStorage (InventoryManager storage)
        {
            this._storage = storage;
        }

        public virtual void PlaceOnScene(Vector3 position, Quaternion rotation)
        { }

        public virtual void AddOnScene (Vector3 position, Quaternion rotation, bool addtocm = true)
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

                if (addtocm)
                {
                    this._AddToCommonManager();
                }
            }

            // переместить объект на позицию спауна
            this._onScene.transform.parent = InventoryItemsManager.Instance.Container;
            this._onScene.transform.position = position;
            this._onScene.transform.rotation = rotation;
        }

        public void AddToHands ()
        {
            InventoryPlayerHands.instance.AddToHands(this);
        }

        public virtual void RemoveFromScene(bool deleteFromCommonManager = true)
        {
            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;
                this.OnDestoyPrefab.Invoke();

                if (deleteFromCommonManager)
                {
                    this._RemoveFromCommonManager();
                }
            }
        }

        public void BeforeChangeManager ()
        {
            this._RemoveFromCommonManager();
        }

        public virtual void Activate()
        {
            
        }

        public virtual void DeActivate ()
        {

        }

        public virtual void LeftClickGameObjectHandler ()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                InventoryManager hands = InventoryPlayerHands.instance.inventoryObject.Manager;

                if ((hands.Inventory[0] != null) && (hands.Inventory[0].Type.Type == "Хранилище"))
                {
                    InventoryItemStorage handsItemStorage = ((InventoryItemStorage)hands.Inventory[0]);
                    
                    if (handsItemStorage.inventory.Add(this))
                    {
                        this.RemoveFromScene();
                    }
                }
                else if (InventoryPlayerHands.instance.inventoryObject.Manager.Add(this))
                {
                    this.RemoveFromScene();
                }
            }
            else if (InventoryPlayerPockets.Instance.Manager.Add(this))
            {
                this.RemoveFromScene();
            }
        }

        public virtual void RightClickGameObjectHandler ()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);
            context.Add("take", () => {
                if (InventoryPlayerHands.instance.inventoryObject.Manager.Add(this))
                {
                    this.RemoveFromScene();
                }
            });
            context.Add("add to pockets", () => {
                if (InventoryPlayerPockets.Instance.Manager.Add(this))
                {
                    this.RemoveFromScene();
                }
            });

            UserInterface.Instance.ContextMenu.Show(context);
        }

        public virtual void LeftClickUIHandler(InventoryManager manager)
        {
            if (manager == InventoryPlayerPockets.Instance.Manager)
            {
                if (InventoryPlayerHands.instance.inventoryObject.Manager.Add(this))
                {
                    manager.Get<InventoryItem>(this);
                }
                else
                {
                    this._SwapItems(this, InventoryPlayerHands.instance.inventoryObject.Manager.Inventory[0]);
                }
            }
            else
            {
                if (manager == InventoryPlayerHands.instance.inventoryObject.Manager)
                {
                    InventoryPlayerHands.instance.Activate();
                }
                else
                {
                    if (InventoryPlayerPockets.Instance.Manager.Add(this))
                    {
                        manager.Get<InventoryItem>(this);
                    }
                }
            }
        }

        public virtual void RightClickUIHandler (InventoryManager manager)
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);

            UserInterface.Instance.ContextMenu.Show(context);
        }

        public virtual void OnHoverGameObjectHandler ()
        {

        }

        public virtual void RemoveFromCommonManager ()
        {
            this._RemoveFromCommonManager();
        }

        public virtual InventoryItemSaveData GetSaveData()
        {
            InventoryItemSaveData saveData = new InventoryItemSaveData()
            {
                amount = this.Amount,
                itemId = this.Type.Id
            };

            if (this.OnScene != null)
            {
                saveData.position = new SerVector(this.OnScene.transform.position);
                saveData.rotation = new SerQuaternion(this.OnScene.transform.rotation);
            }

            if (this.Type.Type == "Хранилище")
            {
                InventoryItemStorage storage = (InventoryItemStorage)(this);
                InventoryManagerSaveData storageSaveData = new InventoryManagerSaveData()
                {
                    managerId = storage.inventory.Type.Id,
                    inventory = new Dictionary<int, InventoryItemSaveData>()
                };

                saveData.inventory = storageSaveData;

                for (int i = 0; i < storage.inventory.Inventory.Count; i++)
                {
                    InventoryItem inventoryItem = storage.inventory.Inventory[i];

                    if (inventoryItem != null)
                    {
                        storageSaveData.inventory.Add(i, inventoryItem.GetSaveData());
                    }
                }
            }
            else if (this.Type.Type == "Робот")
            {
                InventoryItemRobot robot = (InventoryItemRobot)(this);
                InventoryManagerSaveData storageSaveData = new InventoryManagerSaveData()
                {
                    managerId = robot.inventory.Type.Id,
                    inventory = new Dictionary<int, InventoryItemSaveData>()
                };

                saveData.inventory = storageSaveData;

                for (int i = 0; i < robot.inventory.Inventory.Count; i++)
                {
                    InventoryItem inventoryItem = robot.inventory.Inventory[i];

                    if (inventoryItem != null)
                    {
                        storageSaveData.inventory.Add(i, inventoryItem.GetSaveData());
                    }
                }
            }

            return saveData;
        }

        private void _SwapItems (InventoryItem firstItem, InventoryItem secondItem)
        {
            Debug.Log("SwapItems");
            int firstItemPosition = firstItem.Storage.GetPositionByItem(firstItem);
            int secondItemPosition = secondItem.Storage.GetPositionByItem(secondItem);

            Debug.Log("firstItemPosition" + firstItemPosition);
            Debug.Log("secondItemPosition" + secondItemPosition);

            firstItem.Storage.Get<InventoryItem>(firstItem);
            secondItem.Storage.Get<InventoryItem>(secondItem);

            InventoryManager firstManager = firstItem.Storage;
            InventoryManager secondManager = secondItem.Storage;

            secondManager.AddToPosition(secondItemPosition, firstItem);
            firstManager.AddToPosition(firstItemPosition, secondItem);
        }

        protected virtual void _OpenInfo ()
        {
            Window window = UserInterface.Instance.OpenWindow(this._itemType.Name);
            WindowItemInfo itemInfo = MonoBehaviour.Instantiate(
                UserInterface.Instance.ItemInfo,
                window.WindowContainer
            );

            itemInfo.SetData(this._itemType.Icon, this._itemType.Name, new List<ItemPointData>()
            {
                new ItemPointData() { Name = "Количество", Value = this._amount.ToString() },
                new ItemPointData() { Name = "Максимальное", Value = this._itemType.MaxAmount.ToString() },
            });
        }

        private void _OnAmountChange ()
        {
            if (this._amount <= 0)
            {
                this.Removed = true;
                this.RemoveFromScene(true);

                if (this.Storage != null)
                {
                    this.Storage.Get<InventoryItem>(this);
                }

                this.OnDelete.Invoke();
            }
        }

        protected virtual void _AddToCommonManager()
        {
            if (InventoryItemsManager.Instance != null)
            {
                if (!InventoryItemsManager.Instance.Items.Contains(this))
                {
                    if (this.Type.Type == "Хранилище")
                    {
                        // Debug.Log("Add to common manager _ hra");
                    }
                    InventoryItemsManager.Instance.Items.Add(this);
                }
            }
            else
            {
                InventoryItemsManager.OnInit.AddListener((items) =>
                {
                    if (!items.Contains(this))
                    {
                        if (this.Type.Type == "Хранилище")
                        {
                            // Debug.Log("Add to common manager2 _ hra");
                        }
                        items.Add(this);
                    }
                });
            }
        }

        protected virtual void _RemoveFromCommonManager ()
        {
            if (InventoryItemsManager.Instance != null)
            {
                InventoryItemsManager.Instance.Items.Remove(this);
                //Debug.Log("Removed");
            }
            else
            {
                InventoryItemsManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}
