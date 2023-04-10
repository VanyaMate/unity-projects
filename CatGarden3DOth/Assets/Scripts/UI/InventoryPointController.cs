using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VM.InventoryManager;

public class InventoryPointController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _amount;

    private InventoryItemManager _item = null;
    private InventoryManager _manager = null;
    private int _position = -1;
    private bool _dragged = false;

    public InventoryItemManager Item => _item;
    public InventoryManager Manager => _manager;
    public int Position => _position;

    private void OnDestroy ()
    {
        if (this._item != null)
        {
            this.ResetCurrentPoint();
        }
    }

    public void OnBeginDrag (PointerEventData data)
    {
        if (this._item.Data != null)
        {
            this._dragged = true;
            UIManager.Instance.EnableGhostInventoryPoint(this._item);
        }
    }

    public void OnDrag (PointerEventData data)
    {
        if (this._dragged)
        {
            UIManager.Instance.GhostInventoryPoint.transform.position = data.position;
        }
    }

    public void OnEndDrag (PointerEventData data)
    {
        if (this._dragged)
        {
            GameObject UIOverPoint = data.hovered.Find((overPoint) => overPoint.layer == 7);

            if (UIOverPoint)
            {
                if (UIOverPoint.TryGetComponent<InventoryPointController>(out InventoryPointController controller))
                {
                    InventoryItemManager selectedItem = UIManager.Instance.GhostInventoryPoint.Item;

                    if (controller.Item.Data == null)
                    {
                        InventoryItemManager item =
                            selectedItem.Manager.Get(
                                selectedItem
                            );

                        if (!controller.Manager.AddToPosition(item, controller.Position))
                        {
                            this._manager.AddToPosition(item, this._position);
                        }
                    }
                    else if (controller.Item.Data == selectedItem.Data)
                    {
                        controller.Item.Merge(selectedItem);
                    }
                    else if (controller.Item.Data != selectedItem.Data)
                    {
                        controller.Manager.SwapItems(controller.Item, selectedItem);
                    }
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    InventoryItemManager selectedItem = UIManager.Instance.GhostInventoryPoint.Item;

                    if (hit.transform.gameObject.layer == 6)
                    {
                        InventoryItemManager item =
                            selectedItem.Manager.Get(
                                selectedItem
                            );

                        item.AddOnScene(hit.point);
                    }
                    else if (hit.transform.gameObject.layer == 8)
                    {
                        if (hit.transform.TryGetComponent<InventoryManager>(out InventoryManager manager))
                        {
                            InventoryItemManager item =
                                selectedItem.Manager.Get(
                                    selectedItem
                                );

                            manager.Add(item);
                        }
                    }
                }
            }

            UIManager.Instance.DisableGhostInventoryPoint();
            this._dragged = false;
        }
    }

    public void OnPointerEnter (PointerEventData data)
    {

        if (
            // UIManager.Instance.GhostInventoryPoint.gameObject.activeSelf &&
            UIManager.Instance.GhostInventoryPoint.Item != this._item
        )
        {
            this.ActivatePoint();
            // Debug.Log("SetActiveStatus : " + (this._item != null ? this._item.Amount : "empty"));
        }
    }

    public void OnPointerExit (PointerEventData data)
    {

        if (
            // UIManager.Instance.GhostInventoryPoint.gameObject.activeSelf &&
            UIManager.Instance.GhostInventoryPoint.Item != this._item
        )
        {
            this.DeactivatePoint();
            // Debug.Log("DisableActiveStatus : " + (this._item != null ? this._item.Amount : "empty"));
        }
    }

    public void OnPointerUp (PointerEventData data)
    {

    }

    public void SetData(InventoryItemManager item, int position)
    {
        this.ResetCurrentPoint();

        if (item != null)
        {
            this._item = item;
            this._position = position;

            this.UpdateData(item);

            item.OnItemChange.AddListener(this.UpdateData);
            item.OnItemDelete.AddListener(this.OnDeletePoint);
        }
    }

    public void SetManager (InventoryManager manager)
    {
        this._manager = manager;
    }

    public void ResetCurrentPoint ()
    {
        if (this._item != null)
        {
            this._item.OnItemChange.RemoveListener(this.UpdateData);
            this._item.OnItemDelete.RemoveListener(this.OnDeletePoint);

            this._icon.sprite = null;
            this._amount.text = "";

            this._item = null;

            this.DeactivatePoint();
        }
    }

    public void FullResetPoint ()
    {
        if (this._item != null)
        {
            this._item.OnItemChange.RemoveListener(this.UpdateData);
            this._item.OnItemDelete.RemoveListener(this.OnDeletePoint);

            this._icon.sprite = null;
            this._amount.text = "";

            this._item.ResetData();

            this.DeactivatePoint();
        }
    }

    private void ActivatePoint ()
    {
        this._icon.color = new Color(1, 1, 1, 1);
    }

    private void DeactivatePoint ()
    {
        this._icon.color = new Color(.5f, .5f, .5f, .7f);
    }

    private void UpdateData (InventoryItemManager item)
    {
        if (item.Data != null)
        {
            this._icon.sprite = item.Data.Icon;
            this._amount.text = item.Amount.ToString();
        }
    }

    private void OnDeletePoint (InventoryItemManager item)
    {
        this.FullResetPoint();
    }
}
