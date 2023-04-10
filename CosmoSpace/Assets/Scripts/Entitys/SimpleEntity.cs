using CS.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CS
{
    [Serializable]
    public class SimpleEntity
    {
        [Header("Common")]
        [SerializeField] protected SO_SimpleEntity _type;
        [SerializeField] protected CompoundMaterial _insideMaterial;
        [SerializeField] protected CompoundMaterial _geniralMaterial;
        [SerializeField] protected List<CompoundMaterial> _outsideMaterials;
        [SerializeField] protected Vector3 _position;
        [SerializeField] protected Quaternion _rotation;

        [Header("State")]
        [SerializeField] protected float _hp;
        [SerializeField] protected float _temperature;

        protected Dictionary<string, UnityAction> _interactList;
        protected Dictionary<string, UnityAction> _infoList;

        public float hp
        {
            get => this._hp;
            set
            {
                this._hp = value;
                this.OnHealthChange.Invoke(this._hp);
            }
        }
        public float temperature
        {
            get => this._temperature;
            set
            {
                this._temperature = value;
                this.OnTemperatureChange.Invoke(this._temperature);
            }
        }

        public UnityEvent<float> OnHealthChange = new UnityEvent<float>();
        public UnityEvent<float> OnTemperatureChange = new UnityEvent<float>();
        public SO_SimpleEntity type => _type;
        public CompoundMaterial insideMaterial => _insideMaterial;
        public CompoundMaterial geniralMaterial => _geniralMaterial;
        public List<CompoundMaterial> outsideMaterials => _outsideMaterials;
        public Vector3 position => _position;
        public Quaternion rotation => _rotation;    
        public Dictionary<string, UnityAction> interactList => _interactList;
        public Dictionary<string, UnityAction> infoList => _infoList;

        public SimpleEntity(SO_SimpleEntity type)
        {
            this.Init();
        }

        public void Init ()
        {
            this._type = type;
            this._interactList = new Dictionary<string, UnityAction>();
            this._infoList = new Dictionary<string, UnityAction>();
            this._position = Vector3.zero;
            this._rotation = Quaternion.identity;

            this._hp = type.maxHp;
            this._temperature = 24;
        }

        public Material GetMaterial ()
        {
            if (this._outsideMaterials.Count != 0)
            {
                return this._outsideMaterials[^1].GetMaterial();
            }
            else
            {
                return this._geniralMaterial.GetMaterial();
            }
        }
    }
}