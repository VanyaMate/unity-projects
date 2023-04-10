using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.InventoryManager
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private int _inventorySize = 0;
        [SerializeField] private bool _isPlayerPockets = false;

        private Dictionary<int, InventoryItemManager> _inventory = new Dictionary<int, InventoryItemManager>();

        [SerializeField] public string Name;

        public UnityEvent<Dictionary<int, InventoryItemManager>> OnInventoryChange;
        public Dictionary<int, InventoryItemManager> Inventory => _inventory;

        private void Awake()
        {
            if (this._inventorySize != 0)
            {
                for (int i = 0; i < this._inventorySize; i++)
                {
                    this._inventory.Add(i, new InventoryItemManager(null, 0, this));
                }
            }
        }

        private void Start()
        {
            if (this._isPlayerPockets)
            {
                UIManager.Instance.RenderPlayerPockets(this);
                PlayerPockets.Instance.SetPockets(this);
            }
        }

        public void SetInventoryData (string name, int size)
        {
            this.Name = name;
            this._inventorySize = size;
            this._inventory = new Dictionary<int, InventoryItemManager>();

            for (int i = 0; i < this._inventorySize; i++)
            {
                this._inventory.Add(i, new InventoryItemManager(null, 0, this));
            }
        }

        public KeyValuePair<int, InventoryItemManager> Take(InventoryItemManager inventoryItem)
        {
            foreach (KeyValuePair<int, InventoryItemManager> pair in this._inventory)
            {
                if (pair.Value == inventoryItem)
                {
                    return pair;
                }
            }

            return default;
        }

        public InventoryItemManager Get(InventoryItemManager inventoryItem, float amount = -1)
        {
            foreach (KeyValuePair<int, InventoryItemManager> pair in this._inventory)
            {
                if (pair.Value == inventoryItem)
                {
                    if (amount == -1)
                    {
                        this._inventory[pair.Key] = new InventoryItemManager(null, 0, this);
                        this.OnInventoryChange?.Invoke(this._inventory);
                        pair.Value.SetManager(null);
                        return pair.Value;
                    }
                    else
                    {
                        InventoryItemManager item = pair.Value.Get(amount);
                        item.SetManager(null);
                        this.OnInventoryChange?.Invoke(this._inventory);
                        return item;
                    }
                }
            }

            return default;
        }

        public bool Add(InventoryItemManager inventoryItem, bool withoutMerge = false)
        {
            bool addedWithMerge = false;
            bool addedToEmpty = false;

            if (!withoutMerge)
            {
                addedWithMerge = this.AddWithMerge(inventoryItem);
            }

            if (!addedWithMerge)
            {
                addedToEmpty = this.AddToEmpty(inventoryItem);
            }

            return addedWithMerge || addedToEmpty;
        }

        public bool AddToPosition(InventoryItemManager inventoryItem, int position)
        {
            if (this._inventory.TryGetValue(position, out InventoryItemManager manager))
            {
                if (manager.Data == null)
                {
                    this._inventory[position] = inventoryItem;
                    inventoryItem.SetManager(this);
                    this.OnInventoryChange?.Invoke(this._inventory);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddWithMerge(InventoryItemManager inventoryItem)
        {
            HashSet<int> itemNumbers = this.GetItemsInventoryNumbers(inventoryItem);

            if (itemNumbers.Count != 0)
            {
                foreach (int number in itemNumbers)
                {
                    this._inventory.TryGetValue(number, out InventoryItemManager manager);

                    if (manager.Merge(inventoryItem))
                    {
                        this.OnInventoryChange?.Invoke(this._inventory);
                        return true;
                    }
                }
            }

            this.OnInventoryChange?.Invoke(this._inventory);
            return false;
        }

        public bool AddToEmpty(InventoryItemManager inventoryItem)
        {
            HashSet<int> emptyNumbers = this.GetEmptyInventoryNumbers();

            if (emptyNumbers.Count != 0)
            {
                foreach (int number in emptyNumbers)
                {
                    emptyNumbers.TryGetValue(number, out int emptyNumber);
                    this._inventory[emptyNumber] = inventoryItem;
                    inventoryItem.DeleteFromScene(inventoryItem);
                    this.OnInventoryChange?.Invoke(this._inventory);
                    inventoryItem.SetManager(this);
                    return true;
                }
            }

            return false;
        }

        public void SwapItems(InventoryItemManager firstItem, InventoryItemManager secondItem)
        {
            int firstItemPosition = -1;
            int secondItemPosition = -1;

            foreach (KeyValuePair<int, InventoryItemManager> pair in this._inventory)
            {
                if (pair.Value == firstItem)
                {
                    firstItemPosition = pair.Key;
                }
                else if (pair.Value == secondItem)
                {
                    secondItemPosition = pair.Key;
                }
                else if (firstItemPosition != -1 && secondItemPosition != -1)
                {
                    break;
                }
            }

            this._inventory[firstItemPosition] = secondItem;
            this._inventory[secondItemPosition] = firstItem;

            this.OnInventoryChange?.Invoke(this._inventory);
        }

        public bool MergeItems(InventoryItemManager fromItem, InventoryItemManager toItem)
        {
            return toItem.Merge(fromItem);
        }

        private HashSet<int> GetEmptyInventoryNumbers()
        {
            HashSet<int> numbers = new HashSet<int>();

            foreach (KeyValuePair<int, InventoryItemManager> pair in this._inventory)
            {
                if (pair.Value.Data == null)
                {
                    numbers.Add(pair.Key);
                }
            }

            return numbers;
        }

        private HashSet<int> GetItemsInventoryNumbers(InventoryItemManager inventoryItem)
        {
            HashSet<int> numbers = new HashSet<int>();

            foreach (KeyValuePair<int, InventoryItemManager> pair in this._inventory)
            {
                if (pair.Value.Data != null && (pair.Value.Data == inventoryItem.Data))
                {
                    numbers.Add(pair.Key);
                }
            }

            return numbers;
        }
    }
}