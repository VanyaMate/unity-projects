using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VMCode.Inventory;


public class ClickManager : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;

    private void Update()
    {
        Debug.Log(EventSystem.current);
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

            if (hit.transform != null)
            {
                InventoryItemModel inventoryItemModel = hit.transform.GetComponent<InventoryItemModel>();
                InventoryManager inventoryManager = hit.transform.GetComponent<InventoryManager>();

                if (inventoryItemModel)
                {
                    Debug.Log("isInventoryModel");
                    this._inventoryManager.Add(inventoryItemModel.ItemController);              
                }
                else if (inventoryManager)
                {
                    Debug.Log("isChest");
                    UIManager.Instance.OpenInventoryWindow("Chest", inventoryManager);
                }
                else
                {
                    if (UIManager.Instance.MovedInvPoint != false)
                    {
                        InventoryManager manager = UIManager.Instance.MovedPoint.Point.Manager;
                        
                        UIManager.Instance.MovedPoint.Point.Item.SpawnTo(hit.point + new Vector3(0, 1, 0));
                        UIManager.Instance.MovedPoint.Root.RemoveFromClassList("inventoryPointActive");
                        UIManager.Instance.MovedPoint.Point.Item = null;
                        UIManager.Instance.MovedInvPoint = false;
                        UIManager.Instance.UpdateInventoryWindow(manager);
                    }
                }
                    
            }
        }
    }
}
