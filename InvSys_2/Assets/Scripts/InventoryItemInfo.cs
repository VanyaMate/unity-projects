using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "inventoryItem_", menuName = "inventory/item/create", order = 51)]
public class InventoryItemInfo : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;

    public string Name => _itemName;
    public Sprite Icon => _itemIcon;
}
