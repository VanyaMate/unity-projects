using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VM.InventoryManager;

public class ClickManager : MonoBehaviour
{
    private void Update()
    {
        if (
            Input.GetMouseButtonDown(0) && 
            !EventSystem.current.IsPointerOverGameObject()
        )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.TryGetComponent<InventoryManager>(out InventoryManager manager))
                {
                    UIManager.Instance.OpenInventory(manager);
                }
                else if (hit.transform.parent.TryGetComponent<InventoryManager>(out InventoryManager parentManager))
                {
                    UIManager.Instance.OpenInventory(parentManager);
                }
                else if (hit.transform.TryGetComponent<InventoryItem>(out InventoryItem item))
                {
                    PlayerPockets.Instance.Inventory.Add(item.Manager);
                }
            }
        }
    }
}
