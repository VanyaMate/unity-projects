using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CS.SO;
using System;
using UnityEngine.Events;

namespace CS
{
    [Serializable]
    public class SimpleMaterial
    {
        [SerializeField] private SO_SimpleMaterial _material;
        [SerializeField] private float _amount;

        public SO_SimpleMaterial material => _material;
        public float amount {
            get => _amount;
            set
            {
                this._amount = value;
                this.OnAmountChange.Invoke(this._amount);
            }
        }

        public UnityEvent<float> OnAmountChange = new UnityEvent<float>();

        public SimpleMaterial (SO_SimpleMaterial material, float amount)
        {
            this._material = material;
            this._amount = amount;
        }
    }
}