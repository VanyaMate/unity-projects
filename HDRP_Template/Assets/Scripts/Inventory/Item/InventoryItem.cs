using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

        protected InventoryManager _storage;

        public UnityEvent<float> OnAmountChange = new UnityEvent<float>();
        public UnityEvent OnDelete = new UnityEvent();

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
        
        public void SetStorage (InventoryManager storage)
        {
            this._storage = storage;
        }

        public void AddOnScene (Vector3 position, Quaternion rotation)
        {
            // ���� ������ �� ������ - �������
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    this._itemType.Prefab.gameObject, 
                    position, 
                    Quaternion.identity
                );

                this._storage = null;
                this._onScene.GetComponent<InventoryItemObject>().SetManager(this);
                this._AddToCommonManager();
            }

            // ����������� ������ �� ������� ������
            this._onScene.transform.parent = InventoryItemsManager.Instance.Container;
            this._onScene.transform.position = position;
            this._onScene.transform.rotation = rotation;
        }

        public void AddToHands ()
        {
            InventoryPlayerHands.instance.AddToHands(this);
        }

        public void RemoveFromScene(bool deleteFromCommonManager = true)
        {
            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;

                if (deleteFromCommonManager)
                {
                    this._RemoveFromCommonManager();
                }
            }
        }

        public virtual void Activate()
        {
            
        }

        public virtual void DeActivate ()
        {

        }

        public virtual void LeftClickGameObjectHandler ()
        {
            if (InventoryPlayerPockets.Instance.Manager.Add(this))
            {
                this.RemoveFromScene();
            }
        }

        public virtual void RightClickGameObjectHandler ()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);
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

        public virtual void RightClickUIHandler (InventoryManager manager)
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);

            UserInterface.Instance.ContextMenu.Show(context);
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
                new ItemPointData() { Name = "����������", Value = this._amount.ToString() },
                new ItemPointData() { Name = "������������", Value = this._itemType.MaxAmount.ToString() },
            });
        }

        private void _OnAmountChange ()
        {
            if (this._amount == 0)
            {
                InventoryItemsManager.Instance.Items.Remove(this);
                this.OnDelete.Invoke();
            }
        }

        private void _AddToCommonManager()
        {
            if (InventoryItemsManager.Instance != null)
            {
                InventoryItemsManager.Instance.Items.Add(this);
            }
            else
            {
                InventoryItemsManager.OnInit.AddListener((items) => items.Add(this));
            }
        }

        private void _RemoveFromCommonManager ()
        {
            if (InventoryItemsManager.Instance != null)
            {
                InventoryItemsManager.Instance.Items.Remove(this);
            }
            else
            {
                InventoryItemsManager.OnInit.AddListener((items) => items.Remove(this));
            }
        }
    }
}
