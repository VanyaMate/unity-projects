using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VM.Managers
{
    [Serializable]
    public class MoneyManager
    {
        [SerializeField] private float _moneyAmount;

        public UnityEvent<float> OnMoneyChange = new UnityEvent<float>();

        public float money
        {
            get => _moneyAmount;
            set
            {
                _moneyAmount = value;
                OnMoneyChange.Invoke(_moneyAmount);
            }
        }

        public MoneyManager (float amount)
        {
            this.money = amount;
        }

        public bool Get (float amount, out float lacks)
        {
            if (this.money >= amount)
            {
                this.money -= amount;
                lacks = 0;
                return true;
            }
            else
            {
                lacks = amount - this.money;
                return false;
            }
        }

        public void Add (float amount)
        {
            this.money += amount;
        }
    }
}