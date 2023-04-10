using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;

namespace VM.Managers.Customers
{
    public class CustomersManager : MonoBehaviour
    {
        public static CustomersManager instance;

        [SerializeField] private List<Customer> _customersPrefab = new List<Customer>();

        private void Awake()
        {
            instance = this;
        }

        public Customer GetCustomer(bool fillWants = true)
        {
            Customer customer = Instantiate(
                this._customersPrefab[Random.Range(0, this._customersPrefab.Count)],
                transform
            );

            if (fillWants)
            {
                List<CustomerWant> wants = new List<CustomerWant>();
                int coef = InventoryListOfTypes.Instance.items.Count / 3;

                InventoryListOfTypes.Instance.items.ForEach((item) =>
                {
                    if (item.AvailInStore)
                    {
                        float itemCoef = Random.Range(0, InventoryListOfTypes.Instance.items.Count);

                        if (itemCoef < coef)
                        {
                            CustomerWant want = new CustomerWant()
                            {
                                amount = (int)Random.Range(1, 100),
                                itemType = item
                            };

                            wants.Add(want);
                        }
                    }
                });

                customer.SetWants(wants);
            }

            return customer;
        }
    }
}