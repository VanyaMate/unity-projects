using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Seeds;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/seedsItem/create", order = 51)]
    public class SO_InventorySeedsItem : SO_InventoryItem
    {
        [SerializeField] private float _growthTime;
        [SerializeField] private SeedsItemObject _seedlingPrefab;
        [SerializeField] private SO_InventoryItem _seedlingItemType;

        public float growthTime => _growthTime;
        public SeedsItemObject seedlingPrefab => _seedlingPrefab;
        public SO_InventoryItem seedlingItemType => _seedlingItemType;
    }
}