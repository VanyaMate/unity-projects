using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Managers.EntityTools;
using VM.Managers.Road;
using VM.Managers.Store;

namespace VM.Managers.Customers
{
    [Serializable]
    public class CustomerWant
    {
        public float amount;
        public SO_InventoryItem itemType;
    }

    public class CustomerStatus
    {
        public static int Nothing = 0;
        public static int FindGoods = 1;
        public static int GoToStore = 2;
        public static int GoToCar = 3;
        public static int Purchases = 4;
    }

    public class Customer : EntityUnit
    {
        [SerializeField] private List<CustomerWant> _wants = new List<CustomerWant>();
        [SerializeField] private int _status = CustomerStatus.Nothing;

        public List<CustomerWant> wants => _wants;

        private void Start()
        {
            StartCoroutine(this.Task());
        }

        private IEnumerator Task()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);

                if (!this.insideCar)
                {
                    if (this._wants.Count == 0)
                    {
                        this.GoToCar();
                    } 
                    else if (this._status == CustomerStatus.Nothing)
                    {
                        //this._status = CustomerStatus.FindGoods;

                        this._wants.ForEach((want) =>
                        {
                            this.FindStoreWithItem(want);
                            return;
                        });
                    }
                    else if (this._status == CustomerStatus.GoToCar)
                    {
                        this.GoToCar();
                    }
                }
            }
        }

        public void GoToCar ()
        {
            this._status = CustomerStatus.GoToCar;
            this.MoveTo(this._car.transform);
            this.OnStop.AddListener(() =>
            {
                this._car.SeetInside(this);
                this.insideCar = true;
            });
        }

        public void GoToShop (StoreManagerObject store, CustomerWant want)
        {
            this.MoveTo(store.transform);
            this._status = CustomerStatus.GoToStore;
            this.OnStop.AddListener(() =>
            {
                this.OnStop.RemoveAllListeners();
                float purchases = store.PurchaseItems(want.itemType, want.amount);

                if (purchases == want.amount)
                {
                    Debug.Log("eq");
                    this._wants.Remove(want);
                }
                else
                {
                    Debug.Log("!eq");
                    want.amount -= purchases;
                    this.FindStoreWithItem(want);
                }
            });
        }

        private void FindStoreWithItem (CustomerWant want)
        {
            StoreManagerObject store = StoreManager.instance.FindNearestStoreByItem(
                position: transform.position,
                type: want.itemType
            );
            
            if (store != null)
            {
                Debug.Log("Store: " + store.sellingItems.Count);
                this.GoToShop(store, want);
            }
            else
            {
                Debug.Log("GoToCar");
                this._status = CustomerStatus.GoToCar;
            }
        }

        public void SetWants (List<CustomerWant> wants)
        {
            this._wants = wants;
        }

        public void AddWant(CustomerWant want)
        {
            this._wants.Add(want);
        }
    }
}