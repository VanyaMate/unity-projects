using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Seeds;
using VM.TerrainTools;
using VM.UI;
using VM.UI.Path;
using VM.UI.WindowInfo;

namespace VM.Managers.Path
{
    public enum PathType
    { 
        Disabled = -1,
        Path = 0,
        Storage = 1,
        Store = 2,
        Seedling = 3
    }

    public class PathNode : InteractableItem
    {
        [SerializeField] private LineRenderer _lineRendererPrefab;
        [SerializeField] private SO_InventoryPathItem _itemType;

        public List<PathNode> connects = new List<PathNode>();
        public List<InventoryItemObject> storageConnects = new List<InventoryItemObject>();
        public List<Vector3> pointsSeedlingsCheck = new List<Vector3>();
        public List<Vector3> unusabledSeedlingsPlace = new List<Vector3>();
        public List<InventoryItemSeedling> seedlingsItems = new List<InventoryItemSeedling>();
        public List<LineRenderer> lines = new List<LineRenderer>();
        public Dictionary<PathNode, LineRenderer> d_lines = new Dictionary<PathNode, LineRenderer>();
        public Dictionary<InventoryItemObject, LineRenderer> s_lines = new Dictionary<InventoryItemObject, LineRenderer>();
        public int zone = 0;
        public float weight = 9999;
        public PathNode weightFrom;
        public bool isChecked = false;
        public bool showLines = false;
        public float seedlingRadius = 5f;
        public float seedlingInterval = .5f;
        public bool reserved = false;

        public PathType pathNodeType = PathType.Path;

        private void Start()
        {
            this.showLines = PathManager.instance.showLines;
        }

        public void AddConnect (PathNode connect)
        {
            if (!this.connects.Find(x => x == connect))
            {
                this.connects.Add(connect);
                LineRenderer lineRenderer = Instantiate(this._lineRendererPrefab, transform);

                lineRenderer.SetPositions(new Vector3[]
                {
                    transform.position + new Vector3(0, .1f, 0),
                    connect.transform.position + new Vector3(0, .1f, 0)
                });

                lineRenderer.enabled = this.showLines;

                this.d_lines.Add(connect, lineRenderer);
            }
        }

        public void AddStorageConnect (InventoryItemObject storage)
        {
            if (!this.storageConnects.Contains(storage))
            {
                this.storageConnects.Add(storage);
                LineRenderer lineRenderer = Instantiate(this._lineRendererPrefab, transform);

                lineRenderer.SetPositions(new Vector3[]
                {
                    transform.position + new Vector3(0, .1f, 0),
                    storage.transform.position + new Vector3(0, .1f, 0)
                });

                lineRenderer.enabled = this.showLines;
                lineRenderer.endColor = Color.red;
                this.s_lines.Add(storage, lineRenderer);
            }
        }

        public void InitSeedlingNode ()
        {
            this.pointsSeedlingsCheck = new List<Vector3>();
            this.unusabledSeedlingsPlace = new List<Vector3>();
            for (float i = -this.seedlingRadius; i < this.seedlingRadius; i += this.seedlingInterval)
            {
                for (float j = -this.seedlingRadius; j < this.seedlingRadius; j += this.seedlingInterval)
                {
                    if (this.CheckSeedlingPlace(transform.position + new Vector3(i, 0, j), out Vector3 point))
                    {
                        this.unusabledSeedlingsPlace.Add(point);
                    }

                    this.pointsSeedlingsCheck.Add(point);
                }
            }
        }

        public bool CheckSeedlingPlace (Vector3 checkPosition, out Vector3 point, bool ignoreSeedlings = false)
        {
            Vector3 pointCheck = Vector3.up;
            Physics.Raycast(
                pointCheck + checkPosition, 
                Vector3.down, 
                out RaycastHit hit, 
                1.5f,
                ignoreSeedlings ? LayerMask.GetMask("Terrain") : LayerMask.GetMask("Seedling", "Terrain")
            );

            if (hit.transform != null)
            {
                if (hit.transform.gameObject.layer == 7)
                {
                    float groundColorCoef = TerrainManager.Instance.GetColorCoefFrom(
                        position: hit.point,
                        radius: 1f,
                        layer: 1
                    );

                    bool groundCoefGranded = groundColorCoef > .5f;
                    point = hit.point;

                    if (groundCoefGranded)
                    {
                        Debug.DrawLine(pointCheck + checkPosition, hit.point, Color.green, 5f);
                        return true;
                    }
                    else
                    {
                        Debug.DrawLine(pointCheck + checkPosition, hit.point, Color.red, 5f);
                        return false;
                    }
                }
                else
                {
                    point = Vector3.zero;

                    if (hit.transform.gameObject.layer == 13)
                    {
                        if (hit.transform.TryGetComponent<SeedsItemObject>(out SeedsItemObject seedling))
                        {
                            this.seedlingsItems.Add((InventoryItemSeedling)seedling.Manager);
                            Debug.DrawLine(pointCheck + checkPosition, hit.point, Color.yellow, 5f);
                        }
                        else
                        {
                            Debug.DrawLine(pointCheck + checkPosition, hit.point, Color.red, 5f);
                        }
                    }
                    else
                    {
                        Debug.DrawLine(pointCheck + checkPosition, hit.point, Color.red, 5f);
                    }

                    return false;
                }
            }

            point = Vector3.zero;
            return false;
        }

        public void RemoveConnectWith(PathNode connect)
        {
            if (this.connects.Find(x => x == connect))
            {
                this.connects.Remove(connect);
                Destroy(this.d_lines[connect].gameObject);
                this.d_lines.Remove(connect);
            }
        }

        public void RemoveConnects ()
        {
            while (this.connects.Count > 0)
            {
                PathNode connect = this.connects[0];

                this.connects.Remove(connect);
                Destroy(this.d_lines[connect].gameObject);
                this.d_lines.Remove(connect);
                connect.connects.Remove(this);
                Destroy(connect.d_lines[this].gameObject);
                connect.d_lines.Remove(this);
            }
        }

        public void ShowLines (bool value)
        {
            foreach (KeyValuePair<PathNode, LineRenderer> pair in this.d_lines)
            {
                pair.Value.enabled = value;
            }
            this.showLines = value;
        }

        public void Mark ()
        {

        }

        public void SetPathNodeType (PathType type)
        {
            this.pathNodeType = type;

            switch (type)
            {
                case PathType.Seedling:
                    this.InitSeedlingNode();
                    break;

                default:
                    break;
            }
        }

        public override void LeftClickAction()
        {
            this._OpenPathUIMenu();

            if (this.pathNodeType == PathType.Seedling)
            {
                this.InitSeedlingNode();
            }
        }

        public override void RightClickAction()
        {
            Dictionary<string, UnityAction> context = new Dictionary<string, UnityAction>();

            context.Add("info", this._OpenInfo);
            context.Add("open ui", this._OpenPathUIMenu);
            context.Add("take", () => {
                if (InventoryPlayerPockets.Instance.Manager.Add(new InventoryItemPath(type: this._itemType, amount: 1)))
                {
                    PathManager.instance.RemoveNode(this);
                }
            });

            UserInterface.Instance.ContextMenu.Show(context);
        }

        private void _OpenPathUIMenu ()
        {
            PathNodeMenu.instance.SetPathNode(this);
            PathNodeMenu.instance.Show();
        }

        protected virtual void _OpenInfo()
        {
            Window window = UserInterface.Instance.OpenWindow(this._itemType.Name);
            WindowItemInfo itemInfo = MonoBehaviour.Instantiate(
                UserInterface.Instance.ItemInfo,
                window.WindowContainer
            );

            itemInfo.SetData(this._itemType.Icon, this._itemType.Name, new List<ItemPointData>()
            {
                new ItemPointData() { Name = "Количество", Value = "1" },
            });
        }

        public override void HoverAction()
        {
        }
    }
}