using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMCode.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/inventory/item/create", order = 51)]
    public class SO_InventoryItem : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Transform _prefab;
        [SerializeField] private float _amountMax;

        public string Name => _name;
        public Sprite Icon => _icon;
        public Transform Prefab => _prefab;
        public float AmountMax => _amountMax;
    }
}