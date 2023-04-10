using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory.Items;
using VM.UI;
using VM.UI.Inventory;

namespace VM.Inventory
{
    [Serializable]
    public class InventoryManager
    {
        [SerializeField] private SO_InventoryManager _inventoryType;
        [SerializeField] private GameObject _onScene;

        private Dictionary<int, InventoryItem> _inventory = new Dictionary<int, InventoryItem>();

        public SO_InventoryManager Type => _inventoryType;
        public Dictionary<int, InventoryItem> Inventory => this._inventory;
        public GameObject OnScene => _onScene;

        public UnityEvent<Dictionary<int, InventoryItem>> OnInventoryChange = 
            new UnityEvent<Dictionary<int, InventoryItem>>();

        public float reservedSlots = 0;

        public InventoryManager (SO_InventoryManager type, GameObject onScene = null)
        {
            this._inventory = new Dictionary<int, InventoryItem>();
            this._inventoryType = type;
            this._onScene = onScene;

            if (this._onScene != null)
            {
                if (InventoryStoragesManager.Instance != null)
                {
                    InventoryStoragesManager.Instance.Storages.Add(this);
                }
                else
                {
                    InventoryStoragesManager.OnInit.AddListener((storages) => storages.Add(this));
                }
            }

            for (int i = 0; i < this._inventoryType.Size; i++)
            {
                this._inventory[i] = null;
            }
        }

        public bool Add (InventoryItem item)
        {
            /*          bool addToEmptySlot = this.AddToFirstEmptySlot(item);

                        if (!addToEmptySlot)
                        {
                            bool addToSameSlot = this.AddToSameSlot(item);

                            if (!addToSameSlot)
                            {
                                return false;
                            }
                        }

                        item.SetStorage(this);
                        return true;
            */

            bool addToSameSlot = this.AddToSameSlot(item);

            if (!addToSameSlot)
            {
                bool addToEmptySlot = this.AddToFirstEmptySlot(item);

                if (!addToEmptySlot)
                {
                    return false;
                }
            }

            item.SetStorage(this);
            return true;
        }

        public bool AddToPosition (int position, InventoryItem item)
        {
            if (this._CheckItemTypeSuit(item.Type.Type))
            {
                if (this._inventory.TryGetValue(position, out InventoryItem currentItem))
                {
                    if (currentItem == null)
                    {
                        this._inventory[position] = item;
                        item.SetStorage(this);
                        this.OnInventoryChange.Invoke(this._inventory);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public InventoryItem GetItemByPosition (int position)
        {
            this._inventory.TryGetValue(position, out InventoryItem item);
            return item;
        }

        public bool AddToFirstEmptySlot (InventoryItem item)
        {
            if (this._CheckItemTypeSuit(item.Type.Type))
            {
                List<int> emptySlots = this.GetIndexEmptySlots();

                if (emptySlots.Count != 0)
                {
                    foreach (int index in emptySlots)
                    {
                        this._inventory[index] = item;
                        break;
                    }

                    this.OnInventoryChange.Invoke(this._inventory);
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public bool AddToSameSlot (InventoryItem item)
        {
            if (this._CheckItemTypeSuit(item.Type.Type))
            {
                List<int> sameSlots = this.GetIndexSameItems(item.Type);

                if (sameSlots.Count != 0)
                {
                    bool added = false;

                    sameSlots.ForEach((index) =>
                    {
                        if (this._inventory[index].MergeWith(item, out float amount))
                        {
                            added = true;
                            this.OnInventoryChange.Invoke(this._inventory);
                            return;
                        }
                        else
                        {
                            item.Amount = amount;
                        }
                    });

                    // this.OnInventoryChange.Invoke(this._inventory);
                    return added;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public void AddOnScene (Vector3 position, Quaternion rotation)
        {
            // если объект не создан - создать
            if (this._onScene == null)
            {
                this._onScene = MonoBehaviour.Instantiate(
                    this._inventoryType.Prefab.gameObject,
                    position,
                    Quaternion.identity
                );

                this._onScene.transform.parent = InventoryStoragesManager.Instance.Container;
                this._onScene.GetComponent<InventoryManagerObject>().SetManager(this);
            }

            // переместить объект на позицию спауна
            this._onScene.transform.position = position;
            this._onScene.transform.rotation = rotation;
        }

        public InventoryItem Get (int position)
        {
            InventoryItem item = this._inventory[position];

            if (item != null)
            {
                this._inventory[position] = null;
                item.SetStorage(null);
                this.OnInventoryChange.Invoke(this._inventory);
            }

            return item;
        }

        public T Get<T> (InventoryItem item, float amount = -1) where T : InventoryItem
        {
            T _item = default(T);
            int position = -1;

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory[i] == item)
                {
                    position = i;
                    break;
                }
            }

            if (position != -1)
            {
                if (amount != -1)
                {
                    SO_InventoryItem itemType = this._inventory[position].Type;
                    if (this._inventory[position].Get(amount))
                    {
                        object[] props = new object[3];
                        props[0] = itemType;
                        props[1] = amount;
                        _item = (T)Activator.CreateInstance(typeof(T), props);
                    }
                }
                else
                {
                    _item = (T)this._inventory[position];
                    this._inventory[position] = null;
                }

                this.OnInventoryChange.Invoke(this._inventory);
                return _item;
            }
            else
            {
                return null;
            }
        }

        public InventoryItem Get (int position, float amount)
        {
            InventoryItem _item = null;
            InventoryItem item = this._inventory[position];

            if (item != null && item.Amount >= amount)
            {
                if (this._inventory[position].Get(amount))
                {
                    _item = new InventoryItem(item.Type, amount);
                    this.OnInventoryChange.Invoke(this._inventory);
                }
            }

            return _item;
        }

        public float Get (SO_InventoryItem itemType, float amount = -1)
        {
            List<int> sameItems = this.GetIndexSameItems(itemType);
            float gettedAmount = 0;

            for (int i = 0; i < sameItems.Count; i++)
            { 
                float getted = this._inventory[sameItems[i]].GetForce(amount - gettedAmount);
                gettedAmount += getted;

                if (this._inventory[sameItems[i]] != null && this._inventory[sameItems[i]].Amount == 0)
                {
                    this._inventory[sameItems[i]] = null;
                }

                if (gettedAmount == amount)
                {
                    break;
                }
            };

            return gettedAmount;
        }

        public int GetPositionByItem (InventoryItem item)
        {
            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory[i] == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public void LeftClickGameObjectHandler ()
        {
            this.OpenWindow();
        }

        public void RightClickGameObjectHandler ()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("open", this.OpenWindow);
            context.Add("take", () => { Debug.Log("take: " + this._inventoryType.Name); });

            UserInterface.Instance.ContextMenu.Show(context);
        }

        public void LeftClickUIHandler ()
        {

        }

        public void RightClickUIHandler ()
        {

        }

        public void RemoveFromScene ()
        {
            if (this._onScene != null)
            {
                MonoBehaviour.Destroy(this._onScene);
                this._onScene = null;
            }
        }

        public void Remove ()
        {
            this.RemoveFromScene();
            InventoryStoragesManager.Instance.Storages.Remove(this);
        }

        public void FullReset ()
        {
            this._inventory = new Dictionary<int, InventoryItem>();
            this.OnInventoryChange.RemoveAllListeners();
        }

        private bool _CheckItemTypeSuit (string type)
        {
            return 
                (this._inventoryType.ItemsForContainer.Count == 0) ||
                (this._inventoryType.ItemsForContainer.Find((suitType) => suitType.CommonType == type) != null);
        }

        public void OpenWindow()
        {
            Window window = UserInterface.Instance.OpenWindow(this._inventoryType.Name);

            if (this._onScene != null)
            {
                if (this._onScene.TryGetComponent<InventoryManagerObject>(out InventoryManagerObject obj))
                {
                    obj.animator.SetBool("opened", true);
                    window.OnClose.AddListener(() => { obj.animator.SetBool("opened", false); });
                }
            }

            InventoryStorageUI storageUI = MonoBehaviour.Instantiate(
                UserInterface.Instance.InventoryUI,
                window.WindowContainer
            );

            storageUI.SetStorage(this);
        }

        public List<int> GetIndexSameType (string type)
        {
            List<int> sameItems = new List<int>();

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory.TryGetValue(i, out InventoryItem item))
                {
                    if (item != null && item.Type.Type == type)
                    {
                        sameItems.Add(i);
                    }
                }
            }

            return sameItems;
        }

        public float GetAmountItems (string type)
        {
            List<int> itemIndexs = this.GetIndexSameType(type);
            float amount = 0;

            itemIndexs.ForEach((id) =>
            {
                amount += this._inventory[id].Amount;
            });

            return amount;
        }

        public List<int> GetIndexEmptySlots (bool withoutReserved = true)
        {
            List<int> emptySlots = new List<int>();
            float reservedDelta = 0;

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory.TryGetValue(i, out InventoryItem item))
                {
                    if (item == null)
                    {
                        reservedDelta += 1;

                        if ((reservedDelta > this.reservedSlots) || withoutReserved)
                        {
                            emptySlots.Add(i);
                        }
                    }
                }
            }

            return emptySlots;
        }

        public List<int> GetIndexSameItems (SO_InventoryItem itemType)
        {
            List<int> sameItems = new List<int>();

            for (int i = 0; i < this._inventory.Count; i++)
            {
                if (this._inventory.TryGetValue(i, out InventoryItem item))
                {
                    if (item != null && item.Type == itemType)
                    {
                        sameItems.Add(i);
                    }
                }
            }

            return sameItems;
        }
    }
}
