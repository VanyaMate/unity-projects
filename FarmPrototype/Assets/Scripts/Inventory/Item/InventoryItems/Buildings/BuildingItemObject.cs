using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;

namespace VM.Building
{
    public class BuildingItemObject : InventoryItemObject
    {
        [SerializeField] private List<BuildingMagnitBox> _magnitPoints;

        private Vector3 _size;

        public List<BuildingMagnitBox> magnitPoints => _magnitPoints;
        public Vector3 size => _size;

        private void Awake()
        {
            Vector3 size = GetComponent<BoxCollider>().size;
            Vector3 scale = Vector3.one; // this._meshFilter.transform.localScale;

            this._size = new Vector3(size.x * scale.x, size.y * scale.y, size.z * scale.z);
        }
    }
}