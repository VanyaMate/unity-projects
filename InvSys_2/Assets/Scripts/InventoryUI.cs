using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private List<GameObject> _contentItems = new List<GameObject>();

    public void Render (List<InventoryItem> inventoryItems)
    {
        foreach (GameObject item in this._contentItems)
        {
            Destroy(item);
        }

        foreach (InventoryItem item in inventoryItems)
        {
            GameObject invItem = Instantiate(this._itemPrefab, this._content);

            invItem.GetComponent<InventoryUIItem>().SetData(item);

            this._contentItems.Add(invItem);
        }
    }
}
