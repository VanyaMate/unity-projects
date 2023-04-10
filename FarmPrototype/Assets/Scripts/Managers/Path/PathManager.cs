using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Inventory;
using VM.Inventory.Items;
using VM.Managers.Save;
using VM.Save;

namespace VM.Managers.Path
{
    public class PathManager : ObjectToSave
    {
        public static PathManager instance;

        [SerializeField] private PathNode _pathNodePrefab;
        [SerializeField] private List<PathNode> _pathNodes;
        [SerializeField] private Transform _ghost;
        [SerializeField] private LineRenderer _lineRendererPath;
        [SerializeField] private LineRenderer _lineRendererCreateNode;
        [SerializeField] private bool _redactedPathNodes = false;
        [SerializeField] private bool _redactStorageNode = false;

        private PathFinder _pathFinder = new PathFinder();
        private PathNode nearestPathNode = null;
        private Vector3 downPoint;
        private Vector3 upPoint;
        private PathNode startPath = null;
        private PathNode finishPath = null;
        private List<PathNode> path = new List<PathNode>();

        public List<PathNode> pathNodes => _pathNodes;
        public PathFinder pathFinder => _pathFinder;
        public bool redactedPathNode => _redactedPathNodes;
        public bool redactedStorageNode => _redactStorageNode;
        public bool showLines = true;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (this._redactedPathNodes)
            {
                this._RedactPathNodeHandler();
            }
            else if (this._redactStorageNode)
            {
                this._RedactStorageNode();
            }
        }

        public void CreateNode(Vector3 position, PathNode connect = null)
        {
            InventoryManager hands = InventoryPlayerHands.instance.inventoryObject.Manager;

            if (hands.Inventory.Count != 0)
            {
                InventoryItem handItem = hands.Inventory[0];

                if (handItem != null && handItem.Type.Id == 10)
                {
                    InventoryItemPath pathPointItem = (InventoryItemPath)handItem;

                    if (hands.Get<InventoryItemPath>(pathPointItem, 1) != null)
                    {
                        SO_InventoryPathItem pathType = (SO_InventoryPathItem)pathPointItem.Type;
                        PathNode node = Instantiate(pathType.pathNodePrefab, transform);

                        node.transform.position = position;

                        if (connect != null)
                        {
                            node.AddConnect(connect);
                            connect.AddConnect(node);
                        }

                        this._pathNodes.Add(node);
                    }
                }
            }
        }

        public void RemoveNode (PathNode pathNode)
        {
            pathNode.RemoveConnects();
            this.pathNodes.Remove(pathNode);
            Destroy(pathNode.gameObject);
        }

        public void Enable (bool value)
        {
            this._ghost.gameObject.SetActive(value);
            this._redactedPathNodes = value;
        }
    
        public void EnableStorageRedact (bool value)
        {
            this._redactStorageNode = value;
        }

        public void EnableLines (bool value)
        {
            this.showLines = value;
            this._lineRendererPath.enabled = value;
            this._pathNodes.ForEach((node) =>
            {
                node.ShowLines(value);
            });
        }

        public PathNode GetPathNodeStoreWithItemType (string type)
        {
            return new PathNode();
        }

        private PathNode _GetNearestPathNode (Vector3 mousePosition)
        {
            Collider[] colliders = Physics.OverlapSphere(mousePosition, .5f);
            PathNode nearestPathNode = null;

            if (colliders.Length != 0)
            {
                List<PathNode> nodes = new List<PathNode>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.TryGetComponent<PathNode>(out PathNode node))
                    {
                        if (nearestPathNode == null)
                        {
                            nearestPathNode = node;
                            continue;
                        }

                        float distanceToSaved = Vector3.Distance(nearestPathNode.transform.position, mousePosition);
                        float distanceToCurrent = Vector3.Distance(node.transform.position, mousePosition);

                        if (distanceToSaved > distanceToCurrent)
                        {
                            nearestPathNode = node;
                        }
                    }
                }
            }

            return nearestPathNode;
        }

        private void _RedactStorageNode ()
        {
            // add connects to storages
            if (Utils.MouseOverGameObject)
            {
                PathNode nearestPathNode = this._GetNearestPathNode(Utils.MouseWorldPosition.point);

                if (Input.GetMouseButtonDown(0) && nearestPathNode != null && (nearestPathNode.pathNodeType == PathType.Storage || nearestPathNode.pathNodeType == PathType.Store))
                {
                    this.downPoint = Utils.MouseWorldPosition.point;
                    this.nearestPathNode = nearestPathNode;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (this.nearestPathNode != null)
                    {
                        if (Utils.MouseWorldPosition.transform.TryGetComponent<InventoryItemObject>(out InventoryItemObject item))
                        {
                            if (item.Manager.Type.Type == "Хранилище")
                            {
                                this.nearestPathNode.AddStorageConnect(item);
                            }
                        }
                    }
                }
            }
        }

        private void _RedactOutputPoint ()
        {
            // add connects to outputpoints
        }

        private void _RedactSeedlingsNode ()
        {
            // create seedlingsNodes on radius
        }

        private void _RedactPathNodeHandler ()
        {
            if (Utils.MouseOverGameObject)
            {
                this._ghost.position = Utils.MouseWorldPosition.point;

                PathNode nearestPathNode = this._GetNearestPathNode(Utils.MouseWorldPosition.point);

                if (this.nearestPathNode != null)
                {
                    float distance = Vector3.Distance(this.nearestPathNode.transform.position, Utils.MouseWorldPosition.point);

                    if (distance > .5f)
                    {
                        // write line
                        this._lineRendererCreateNode.gameObject.SetActive(true);
                        this._lineRendererCreateNode.SetPositions(new Vector3[]
                        {
                            this.nearestPathNode.transform.position + new Vector3(0, .1f, 0),
                            Utils.MouseWorldPosition.point + new Vector3(0, .1f, 0)
                        });
                    }
                    else
                    {
                        this._lineRendererCreateNode.gameObject.SetActive(false);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    this.downPoint = Utils.MouseWorldPosition.point;
                    this.nearestPathNode = nearestPathNode;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    this.upPoint = Utils.MouseWorldPosition.point;
                    float distance = Vector3.Distance(this.downPoint, this.upPoint);

                    if ((distance > .5f) && (this.nearestPathNode != null))
                    {
                        if (nearestPathNode != null)
                        {
                            nearestPathNode.AddConnect(this.nearestPathNode);
                            this.nearestPathNode.AddConnect(nearestPathNode);
                        }
                        else
                        {
                            this.CreateNode(this.upPoint, this.nearestPathNode);
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftControl))
                    {
                        this.CreateNode(this.upPoint);
                    }

                    this._lineRendererCreateNode.gameObject.SetActive(false);
                    this.nearestPathNode = null;
                    this.downPoint = default;
                    this.upPoint = default;
                }

                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    if (nearestPathNode != null)
                    {
                        this.startPath = nearestPathNode;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    if (nearestPathNode != null)
                    {
                        this.finishPath = nearestPathNode;
                        this.path = PathFinder.GetPathFromTo(this.startPath, this.finishPath);
                        Vector3[] pathPoints = this.path.ConvertAll(x => x.transform.position + new Vector3(0, .5f, 0)).ToArray();

                        this._lineRendererPath.positionCount = pathPoints.Length;
                        this._lineRendererPath.SetPositions(pathPoints);

                        //Robots.RobotManager.instance.SetPath(this.path);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    PathFinder.ResetNodes(this._pathNodes);
                    this.path = new List<PathNode>();
                    this._lineRendererPath.positionCount = 0;
                    this._lineRendererPath.SetPositions(new Vector3[] { });
                }
            }

            this._pathNodes.ForEach((node) =>
            {
                node.connects.ForEach((connect) =>
                {
                    Debug.DrawLine(node.transform.position, connect.transform.position, Color.blue);
                });
            });

            PathNode prevPathNode = null;
            this.path.ForEach((node) =>
            {
                if (prevPathNode != null)
                {
                    Debug.DrawLine(node.transform.position + Vector3.up, prevPathNode.transform.position + Vector3.up, Color.red);
                }

                prevPathNode = node;
            });
        }

        public override string GetSaveData()
        {
            List<PathNodeSaveData> pathNodes = new List<PathNodeSaveData>();

            this._pathNodes.ForEach((node) =>
            {
                PathNodeSaveData nodeSave = new PathNodeSaveData()
                {
                    position = new SerVector(node.transform.position),
                    itemId = 10
                };

                List<SerVector> connections = new List<SerVector>();
                List<SerVector> storageConnections = new List<SerVector>();

                node.connects.ForEach((connect) =>
                {
                    connections.Add(new SerVector(connect.transform.position));
                });

                node.storageConnects.ForEach((connect) =>
                {
                    storageConnections.Add(new SerVector(connect.transform.position));
                });

                nodeSave.type = node.pathNodeType;
                nodeSave.connections = connections.ToArray();
                nodeSave.storageConnections = storageConnections.ToArray();

                pathNodes.Add(nodeSave);
            });

            return JsonConvert.SerializeObject(pathNodes);
        }

        public override void LoadSaveData(string data)
        {
            List<PathNodeSaveData> pathNodes = JsonConvert.DeserializeObject<List<PathNodeSaveData>>(data);
            Dictionary<PathNodeSaveData, PathNode> paths = new Dictionary<PathNodeSaveData, PathNode>();
            this._pathNodes.ForEach((x) => Destroy(x.gameObject));
            this._pathNodes = new List<PathNode>();

            // init all nodes
            pathNodes.ForEach((node) =>
            {
                SO_InventoryPathItem nodeType = (SO_InventoryPathItem)InventoryListOfTypes.Instance.GetItemById(10);
                PathNode instantiateNode = Instantiate(nodeType.pathNodePrefab, transform);

                instantiateNode.transform.position = new UnityVector3(node.position).vector;
                instantiateNode.pathNodeType = node.type;

                if (instantiateNode.pathNodeType == PathType.Storage || instantiateNode.pathNodeType == PathType.Store)
                {
                    for (int i = 0; i < node.storageConnections.Length; i++)
                    {
                        SerVector connectionPosition = node.storageConnections[i];
                        Vector3 position = new UnityVector3(connectionPosition).vector;
                        Collider[] findedColliderFromPosition = Physics.OverlapSphere(position, .5f, LayerMask.GetMask("InventoryItem"));
                        Collider nearestCollider = default;
                        float distance = 99999;

                        for (int j = 0; j < findedColliderFromPosition.Length; j++)
                        {
                            Collider findedCollider = findedColliderFromPosition[i];
                            float distanceToCurrentCollider = Vector3.Distance(findedCollider.transform.position, position);

                            if (distanceToCurrentCollider < distance)
                            {
                                nearestCollider = findedCollider;
                                distance = distanceToCurrentCollider;
                            }
                        }

                        if (distance != 99999 && nearestCollider.TryGetComponent<InventoryItemObject>(out InventoryItemObject itemObject))
                        {
                            if (itemObject.Manager.Type.Type == "Хранилище")
                            {
                                instantiateNode.storageConnects.Add(itemObject);
                                break;
                            }
                        }
                    }
                }
                else if (instantiateNode.pathNodeType == PathType.Seedling)
                {
                    instantiateNode.InitSeedlingNode();
                }

                paths.Add(node, instantiateNode);

                this._pathNodes.Add(instantiateNode);
            });

            // find connections
            pathNodes.ForEach((node) =>
            {
                for (int i = 0; i < node.connections.Length; i++)
                {
                    SerVector connectionPosition = node.connections[i];
                    Vector3 position = new UnityVector3(connectionPosition).vector;

                    PathNode conPathNode = this._pathNodes.Find((x) => Vector3.Distance(position, x.transform.position) < .1f);

                    if (conPathNode != null)
                    {
                        paths[node].AddConnect(conPathNode);
                        conPathNode.AddConnect(paths[node]);
                    }
                }
            });
        }
    }
}