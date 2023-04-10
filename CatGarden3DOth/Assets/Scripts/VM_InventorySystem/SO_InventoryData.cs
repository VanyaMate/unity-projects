using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.InventoryManager
{
    [CreateAssetMenu(fileName = "so_inventoryData_", menuName = "game/inventoryItem/create", order = 51)]
    public class SO_InventoryData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _amountMax;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Transform _prefab;

        public string Name => _name;
        public float AmountMax => _amountMax;
        public Sprite Icon => _icon;
        public Transform Prefab => _prefab;
    }
}
