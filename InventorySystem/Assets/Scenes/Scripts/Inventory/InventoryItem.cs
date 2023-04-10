using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VMCode.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private SO_InventoryItem _itemData;
        [SerializeField] private float _amount;
        [SerializeField] private InventoryItemModel _itemOnScene;

        public SO_InventoryItem Type => _itemData;
        public float Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        public InventoryItem(SO_InventoryItem itemData, float amount = 1)
        {
            this._itemData = itemData;
            this._amount = amount;
        }

        public void SpawnTo(Vector3 point)
        {
            if (this._itemOnScene == null)
            {
                this._itemOnScene = MonoBehaviour.Instantiate(this._itemData.Prefab, Vector3.zero, Quaternion.identity).GetComponent<InventoryItemModel>();
                this._itemOnScene.transform.position = point;
                this._itemOnScene.SetController(this);
            }
        }

        public void DeleteFromScene()
        {
            if (this._itemOnScene != null)
            {
                MonoBehaviour.Destroy(this._itemOnScene.gameObject);
                this._itemOnScene = null;
            }
        }
    }
}