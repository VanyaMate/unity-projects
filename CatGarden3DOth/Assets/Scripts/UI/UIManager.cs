using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.InventoryManager;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Prefabs")]
    [Header("Window")]
    [SerializeField] private Transform _window;

    [Header("Inventory")]
    [SerializeField] private Transform _inventoryContentForWindow;
    [SerializeField] private Transform _inventoryPoint;
    [SerializeField] private Transform _inventoryPocketContainer;

    private InventoryPointController _ghostInventoryPoint;

    public InventoryPointController GhostInventoryPoint => _ghostInventoryPoint;

    private void Awake()
    {
        Instance = this;

        this._ghostInventoryPoint = 
            Instantiate(this._inventoryPoint, transform)
            .GetComponent<InventoryPointController>();
        this._ghostInventoryPoint.gameObject.SetActive(false);
        this._ghostInventoryPoint.gameObject.layer = 2;
    }
    
    public void EnableGhostInventoryPoint (InventoryItemManager manager)
    {
        this._ghostInventoryPoint.SetData(manager, -1);
        this._ghostInventoryPoint.gameObject.SetActive(true);
    }

    public void DisableGhostInventoryPoint ()
    {
        this._ghostInventoryPoint.ResetCurrentPoint();
        this._ghostInventoryPoint.gameObject.SetActive(false);
    }

    public void OpenInventory (InventoryManager inventoryManager)
    {
        WindowManager window = this.OpenWindow(inventoryManager.Name);

        InventoryContentManager inventoryContent = Instantiate(
            this._inventoryContentForWindow, 
            window.transform
        ).GetComponent<InventoryContentManager>();

        List<InventoryPointController> pointControllers = 
            this.FillInventoryPoints(
                inventoryManager.Inventory, 
                inventoryContent.Content
            );

        UnityAction<Dictionary<int, InventoryItemManager>> onChangeEvent = 
            (Dictionary<int, InventoryItemManager> inventory) => {
                this.UpdateInventoryPoints(
                    inventory, 
                    pointControllers
                );
            };

        UnityAction onWindowClose = () =>
        {
            inventoryManager.OnInventoryChange.RemoveListener(onChangeEvent);
        };

        inventoryManager.OnInventoryChange.AddListener(onChangeEvent);
        window.OnDestroyEvent.AddListener(onWindowClose);
    }

    public void RenderPlayerPockets (InventoryManager inventoryPocketManager)
    {
        Transform pockets = Instantiate(this._inventoryPocketContainer, transform);

        List<InventoryPointController> pointControllers =
            this.FillInventoryPoints(
                inventoryPocketManager.Inventory,
                pockets
            );

        UnityAction<Dictionary<int, InventoryItemManager>> onChangeEvent =
            (Dictionary<int, InventoryItemManager> inventory) => {
                this.UpdateInventoryPoints(
                    inventory,
                    pointControllers
                );
            };

        ((RectTransform)pockets).sizeDelta = new Vector2(pointControllers.Count * 55f, 60);

        inventoryPocketManager.OnInventoryChange.AddListener(onChangeEvent);
    }

    private void UpdateInventoryPoints (
        Dictionary<int, InventoryItemManager> inventory, 
        List<InventoryPointController> controllers
    )
    {
        int i = 0;
        foreach (InventoryPointController controller in controllers)
        {
            controller.SetData(inventory.GetValueOrDefault(i), i);
            i += 1;
        }
    }

    private List<InventoryPointController> FillInventoryPoints (
        Dictionary<int, InventoryItemManager> inventory, 
        Transform content
    )
    {
        List<InventoryPointController> pointControllers = new List<InventoryPointController>();

        foreach (KeyValuePair<int, InventoryItemManager> pair in inventory)
        {
            Transform inventoryPoint = Instantiate(this._inventoryPoint, content);
            InventoryPointController controller = inventoryPoint.GetComponent<InventoryPointController>();

            controller.SetData(pair.Value, pair.Key);
            controller.SetManager(pair.Value != null ? pair.Value.Manager : null);
            pointControllers.Add(controller);
        }

        return pointControllers;
    }

    private WindowManager OpenWindow (string windowName)
    {
        WindowManager window = Instantiate(this._window, transform).GetComponent<WindowManager>();
        window.SetName(windowName);
        return window;
    }
}