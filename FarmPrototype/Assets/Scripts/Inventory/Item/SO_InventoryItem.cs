using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Inventory
{
    [CreateAssetMenu(fileName = "so_inventoryItem_", menuName = "game/create/inventory/inventoryItem/create", order = 51)]
    public class SO_InventoryItem : ScriptableObject
    {
        [SerializeField] private string _commonType;
        [SerializeField] private string _type;
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _maxAmount;
        [SerializeField] private float _cost;
        [SerializeField] private InventoryItemObject _prefab;
        [SerializeField] private GameObject _clearPrefab;
        [SerializeField] private bool _availableInStore;

        public string CommonType => _commonType;
        public string Type => _type;
        public int Id => _id;
        public string Name => _name;
        public Sprite Icon => _icon;
        public float MaxAmount => _maxAmount;
        public float Cost => _cost;
        public InventoryItemObject Prefab => _prefab;
        public GameObject ClearPrefab => _clearPrefab;
        public bool AvailInStore => _availableInStore;
    }
}
