using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryUIItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemAmount;
    [SerializeField] private InventoryItem _item;

    private GameObject _preview;

    public void SetData (InventoryItem item)
    {
        this._item = item;
        this._itemIcon.sprite = item.ItemInfo.Icon;
        this._itemName.text = item.ItemInfo.Name;
        this._itemAmount.text = item.Amount.ToString();
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (this._preview)
        {
            Destroy(this._preview);
            this._preview = null;
        }

        this._preview = Instantiate(this._item.Prefab, Vector3.zero, Quaternion.identity);
        this._preview.GetComponent<InventoryItem>().Preview();
    }

    private void Update()
    {
        if (this._preview)
        {
            Plane plane = new Plane(Vector3.up, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (plane.Raycast(ray, out distance))
            {
                this._preview.transform.position = ray.GetPoint(distance);
            }
        }
    }
}
