using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VMCode.Inventory
{
    [Serializable]
    public class InventoryPoint
    {
        public InventoryItem Item;
        public int Position;
        public InventoryManager Manager;
    }

    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private bool _isPlayerPockets;
        [SerializeField] private int _inventorySize;
        [SerializeField] private List<InventoryPoint> _inventory;

        public List<InventoryPoint> Inventory => _inventory;
        public UnityEvent OnInventoryChange;

        private void Awake()
        {
            this._inventory = new List<InventoryPoint>();

            for (int i = 0; i < this._inventorySize; i++)
            {
                this._inventory.Add(new InventoryPoint() { Item = null, Position = i, Manager = this });
            }
        }

        private void Start()
        {
            if (this._isPlayerPockets)
            {
                UIManager.Instance.RenderBottomCenterPoints(this);
            }
        }

        private void Update()
        {

        } 

        public bool Add (InventoryItem item, int position = -1)
        {
            List<InventoryPoint> pointsByItem = this.GetPointsByItem(item);
            float amountMax = item.Type.AmountMax;

            if (position != -1)
            {
                InventoryPoint inventoryPoint = this._inventory.Find((InventoryPoint point) => point.Position == position);
                if (inventoryPoint.Item != null)
                {
                    return false;
                }
                else 
                {
                    inventoryPoint.Item = item;
                    return true;
                }
            }

            if (pointsByItem.Count != 0)
            {
                foreach (InventoryPoint point in pointsByItem)
                {
                    if (point.Item.Amount < amountMax)
                    {
                        float delta = amountMax - point.Item.Amount;
                       
                        if (item.Amount <= delta)
                        {
                            point.Item.Amount += item.Amount;
                            item.DeleteFromScene();
                            this.OnInventoryChange?.Invoke();
                            return true;
                        }
                        else
                        {
                            point.Item.Amount += delta;
                            item.Amount -= delta;
                        }
                    }
                }
            }

            List<InventoryPoint> emptyPoints = this.GetEmptyPoints();
            
            if (emptyPoints.Count != 0)
            {
                foreach (InventoryPoint point in emptyPoints)
                {
                    if (item.Amount <= amountMax)
                    {
                        point.Item = item;
                        item.DeleteFromScene();
                        this.OnInventoryChange?.Invoke();
                        return true;
                    }
                    else
                    {
                        point.Item = new InventoryItem(item.Type, amountMax);
                        item.Amount -= amountMax;
                    }
                }
            }

            this.OnInventoryChange?.Invoke();
            return false;
        }

        public InventoryItem Get (InventoryItem item)
        {
            InventoryPoint inventoryPoint = this._inventory.Find((InventoryPoint x) => x.Item != null && x.Item.Type == item.Type);

            if (inventoryPoint != null)
            {
                InventoryItem inventoryItem = inventoryPoint.Item;
                inventoryPoint.Item = null;
                this.OnInventoryChange?.Invoke();
                return inventoryItem;
            }
            else 
            {
                return null;
            }
        }

        public InventoryItem Get (int position)
        {
            InventoryPoint inventoryPoint = this._inventory.Find((InventoryPoint x) => x.Item != null && x.Position == position);

            if (inventoryPoint != null)
            {
                InventoryItem inventoryItem = inventoryPoint.Item;
                inventoryPoint.Item = null;
                this.OnInventoryChange?.Invoke();
                return inventoryItem;
            }
            else
            {
                return null;
            }
        }

        private List<InventoryPoint> GetEmptyPoints (bool first = false)
        {
            return first ? 
                new List<InventoryPoint> { 
                    this._inventory.Find((InventoryPoint x) => x.Item == null || (x.Item != null && x.Item.Type == null)) 
                } : 
                this._inventory.FindAll((InventoryPoint x) => x.Item == null || (x.Item != null && x.Item.Type == null));
        }

        private List<InventoryPoint> GetPointsByItem(InventoryItem item)
        {
            return this._inventory.FindAll((InventoryPoint x) => x.Item != null && x.Item.Type == item.Type);
        }
    }
}
