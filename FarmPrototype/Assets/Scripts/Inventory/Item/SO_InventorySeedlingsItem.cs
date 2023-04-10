using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/seedlingItem/create", order = 51)]
    public class SO_InventorySeedlingsItem : SO_InventoryItem
    {
        [SerializeField] private float _growthTime;
        [SerializeField] private InventoryItemObject _seedlingItemPrefab;
        [SerializeField] private SO_InventoryItem _finalItem;

        public float growthTime => _growthTime;
        public InventoryItemObject seedlingItemPrefab => _seedlingItemPrefab;
        public SO_InventoryItem finalItem => _finalItem;
    }
}