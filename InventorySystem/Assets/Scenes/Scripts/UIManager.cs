using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using VMCode.Inventory;

public class InventoryPointRender
{
    public VisualElement Root;
    public VisualElement Icon;
    public Label Amount;
    public InventoryPoint Point;
}

public class WindowData
{
    public string Name;
    public VisualElement Content;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("Common")]
    [SerializeField] private UIDocument _document;
    [SerializeField] private VisualTreeAsset _window;

    [Header("Inventory")]
    [SerializeField] private VisualTreeAsset _inventoryPoint;
    [SerializeField] private int _inventorySize;

    private VisualElement _root;

    // bottomCenter
    private Dictionary<int, InventoryPointRender> _bottomCenterPoints = 
        new Dictionary<int, InventoryPointRender>();
    private VisualElement _bottomCenter;

    // inventory
    private Dictionary<InventoryManager, List<InventoryPointRender>> _inventoryWindows = 
        new Dictionary<InventoryManager, List<InventoryPointRender>>();

    // movedWindow
    private VisualElement _movedWindow;
    private Vector2 _startPosition;
    private Vector2 _startMousePosition;

    // moveInventoryPoints
    private bool _movedInvPoint = false;
    private InventoryPointRender _movedPoint;

    public bool MovedInvPoint
    {
        get => _movedInvPoint;
        set => _movedInvPoint = value;
    }
    public InventoryPointRender MovedPoint => _movedPoint;

    private void Awake()
    {
        Instance = this;

        this._root = this._document.rootVisualElement;
        this._bottomCenter = this._root.Q<GroupBox>("bottomCenter");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    public void RenderBottomCenterPoints (InventoryManager inventoryManager)
    {
        this.RenderInventoryPoints(inventoryManager, this._bottomCenter);
    }

    public void OpenInventoryWindow (string title, InventoryManager inventoryManager)
    {
        List<InventoryPointRender> renderedPoints;
        this._inventoryWindows.TryGetValue(inventoryManager, out renderedPoints);

        if (renderedPoints == null)
        {
            // Window
            VisualElement windowTemplate = this._window.CloneTree();
            GroupBox window = windowTemplate.Q<GroupBox>("window");
            GroupBox topWindow = window.Q<GroupBox>("topWindow");
            Label windowTitle = window.Q<Label>("topWindowTitle");
            Button windowCloseButton = window.Q<Button>("topWindowCloseButton");
            GroupBox windowContentBox = window.Q<GroupBox>("contentWindow");
            UnityAction updateItemsEvent = () => this.UpdateInventoryWindow(inventoryManager);
            EventCallback<MouseMoveEvent> mouseMoveEvent = (MouseMoveEvent evt) =>
            {
                if (this._movedWindow != null)
                {
                    this._movedWindow.transform.position = this._startPosition - this._startMousePosition + evt.mousePosition;
                }
            };
            EventCallback<MouseDownEvent> mouseDownEvent = (MouseDownEvent evt) =>
            {
                this._startMousePosition = evt.mousePosition;
                this._startPosition = window.transform.position;
                this._movedWindow = window;
            };            
            EventCallback<MouseUpEvent> mouseUpEvent = (MouseUpEvent evt) =>
            {
                this._movedWindow = null;
            };

            window.RegisterCallback<MouseMoveEvent>(mouseMoveEvent);
            topWindow.RegisterCallback<MouseDownEvent>(mouseDownEvent);
            topWindow.RegisterCallback<MouseUpEvent>(mouseUpEvent);
            
            windowTitle.text = title;
            windowCloseButton.clicked += () =>
            {
                windowCloseButton.Blur();
                inventoryManager.OnInventoryChange.RemoveListener(updateItemsEvent);
                window.UnregisterCallback<MouseMoveEvent>(mouseMoveEvent);
                topWindow.UnregisterCallback<MouseDownEvent>(mouseDownEvent);
                topWindow.UnregisterCallback<MouseUpEvent>(mouseUpEvent);
                this._root.Remove(window);
                this._inventoryWindows.Remove(inventoryManager);
            };

            this.RenderInventoryPoints(inventoryManager, windowContentBox);

            this._root.Add(window);
        }
    }

    private void RenderInventoryPoints (InventoryManager inventoryManager, VisualElement container)
    {
        UnityAction updateItemsEvent = () => this.UpdateInventoryWindow(inventoryManager);
        EventCallback<MouseDownEvent> mouseDownEvent = (MouseDownEvent evt) =>
        {

        };

        inventoryManager.OnInventoryChange.AddListener(updateItemsEvent);
        this._inventoryWindows.Add(inventoryManager, new List<InventoryPointRender>());

        for (int i = 0; i < inventoryManager.Inventory.Count; i++)
        {
            int _i = i;
            VisualElement inventoryPointTemplate = this._inventoryPoint.CloneTree();
            GroupBox inventoryPoint = inventoryPointTemplate.Q<GroupBox>("inventoryPoint");

            inventoryPoint.RegisterCallback<MouseDownEvent>((MouseDownEvent evt) => 
            {
                InventoryPointRender point = this._inventoryWindows[inventoryManager][_i];
                if (this._movedInvPoint == false && point != null && point.Point != null && point.Point.Item != null)
                {
                    this._movedPoint = point;
                    this._movedInvPoint = true;
                    point.Root.AddToClassList("inventoryPointActive");
                }
            });
            inventoryPoint.RegisterCallback<MouseUpEvent>((MouseUpEvent evt) =>
            {
                InventoryPointRender point = this._inventoryWindows[inventoryManager][_i];
                if (this._movedInvPoint && this._movedPoint != null && this._movedPoint.Point != null)
                {
                    if (this._movedPoint.Point != point.Point)
                    {
                        // Merge
                        if (this._movedPoint.Point.Item != null && point.Point.Item != null && (this._movedPoint.Point.Item.Type == point.Point.Item.Type))
                        {
                            InventoryPoint invPoint = point.Point.Manager.Inventory.Find(i => i.Position == (point.Point != null ? point.Point.Position : _i));
                            float maxAmount = invPoint.Item.Type.AmountMax;
                            float delta = maxAmount - invPoint.Item.Amount;

                            if (delta >= this._movedPoint.Point.Item.Amount)
                            {
                                invPoint.Item.Amount += this._movedPoint.Point.Item.Amount;
                                this._movedPoint.Point.Item = null;
                            }
                            else
                            {
                                invPoint.Item.Amount += delta;
                                this._movedPoint.Point.Item.Amount -= delta;
                            }

                            this._movedPoint.Root.RemoveFromClassList("inventoryPointActive");
                        }
                        // Swap
                        else
                        {
                            InventoryPoint invPoint = point.Point.Manager.Inventory.Find(i => i.Position == (point.Point != null ? point.Point.Position : _i));
                            InventoryItem item = this._movedPoint.Point.Item;

                            this._movedPoint.Root.RemoveFromClassList("inventoryPointActive");
                            this._movedPoint.Point.Item = point.Point.Item;
                            invPoint.Item = item;
                        }

                        this.UpdateInventoryWindow(this._movedPoint.Point.Manager);
                        this.UpdateInventoryWindow(inventoryManager);
                        this._movedInvPoint = false;
                    }
                }
            });

            this._inventoryWindows[inventoryManager].Add(new InventoryPointRender()
            {
                Icon = inventoryPoint.Q<VisualElement>("inventoryPointIcon"),
                Amount = inventoryPoint.Q<Label>("inventoryPointAmount"),
                Point = new InventoryPoint() { Manager = inventoryManager, Position = _i },
                Root = inventoryPoint
            });

            container.Add(inventoryPoint);
        }

        this.UpdateInventoryWindow(inventoryManager);
    }

    public void UpdateInventoryWindow (InventoryManager inventoryManager)
    {
        int i = 0;
        foreach (InventoryPoint inventoryPoint in inventoryManager.Inventory)
        {
            if (inventoryPoint.Item != null && inventoryPoint.Item.Amount != 0)
            {
                InventoryPointRender renderedPoint = this._inventoryWindows[inventoryManager][inventoryPoint.Position];
                renderedPoint.Amount.text = inventoryPoint.Item.Amount.ToString();
                renderedPoint.Icon.style.backgroundImage = new StyleBackground(inventoryPoint.Item.Type.Icon);
                renderedPoint.Point = inventoryPoint;
            }
            else
            {
                InventoryPointRender renderedPoint = this._inventoryWindows[inventoryManager][inventoryPoint.Position];
                renderedPoint.Amount.text = "";
                renderedPoint.Icon.style.backgroundImage = null;
                renderedPoint.Point = new InventoryPoint() { Manager = inventoryManager, Position = i };
            }

            i += 1;
        }
    }
}
