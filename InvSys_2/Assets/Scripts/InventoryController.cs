using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> _storage = new List<InventoryItem>();
    [SerializeField] private InventoryUI _inventoryUI;

    public void AddItem (InventoryItem item)
    {
        InventoryItem sameStorageItem = this._storage.Find(e => e.ItemInfo.Name == item.ItemInfo.Name);

        if (sameStorageItem)
        {
            sameStorageItem.Amount += item.Amount;
            Destroy(item.gameObject);
        }
        else
        {
            this._storage.Add(item);

            item.HideTo(transform);
        }

        if (this._inventoryUI)
        {
            this._inventoryUI.Render(this._storage);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InventoryItem>(out InventoryItem item))
        {
            this.AddItem(item);
        }
    }

    public void TakeItem ()
    {

    }
}
