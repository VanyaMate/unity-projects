using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Bson;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    
    [SerializeField] private Transform _buildingMap;
    [SerializeField] private List<BuildingTypeSO> _buildingTypes = new List<BuildingTypeSO>();
    [SerializeField] private Transform _ghostBuilding;

    private Transform _tempGhostBuild;
    private BuildingTypeSO _currentGhostBuild;
    private Vector3 _lastGhostPosition;

    private HashSet<Transform> _buildingHashList = new HashSet<Transform>();
    
    public Transform GhostBuilding => _ghostBuilding;
    public HashSet<Transform> BuildingHashList => _buildingHashList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this._buildingHashList.Add(PlayerController.Instance.transform);
    }

    private void Update()
    {
        if (this._ghostBuilding.gameObject.activeSelf)
        {
            RaycastHit currentWorldHit = GameUtils.Instance.GetMouseWorldPositionHit();

            if (currentWorldHit.collider.name == "Map")
            {
                this._lastGhostPosition = currentWorldHit.point;
                this._tempGhostBuild?.gameObject.SetActive(true);
            }
            else
            {
                this._tempGhostBuild?.gameObject.SetActive(false);
            }

            if (this._lastGhostPosition != null)
            {
                this._ghostBuilding.position = this._lastGhostPosition;
            }

            if (this._tempGhostBuild?.gameObject.activeSelf == true)
            {
                if (
                    !EventSystem.current.IsPointerOverGameObject() &&
                    Input.GetMouseButtonDown(0)
                )
                {
                    this.Build();
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    this._tempGhostBuild.rotation = Quaternion.Euler(
                        0,
                        this._tempGhostBuild.transform.eulerAngles.y - 30,
                        0
                    );
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    this._tempGhostBuild.rotation = Quaternion.Euler(
                        0,
                        this._tempGhostBuild.transform.eulerAngles.y + 30,
                        0
                    );
                }
            }
        }
    }

    public void ActivateGhostBuilding(BuildingTypeSO buildingTypeSo)
    {
        this.DeativateGhostBuilding();
        this._ghostBuilding.gameObject.SetActive(true);
        this._currentGhostBuild = buildingTypeSo;
        this._tempGhostBuild = Instantiate(buildingTypeSo.PrefabModel, this._ghostBuilding);
        this._tempGhostBuild.localPosition = new Vector3(0, this._tempGhostBuild.localScale.y / 2, 0);
    }

    public void DeativateGhostBuilding()
    {
        if (this._tempGhostBuild != null)
        {
            Destroy(this._tempGhostBuild.gameObject);
            this._tempGhostBuild = null;    
            this._currentGhostBuild = null;
        }

        this._ghostBuilding.gameObject.SetActive(false);
    }

    public void Build()
    {
        Transform build = Instantiate(this._currentGhostBuild.Prefab, this._buildingMap);
        build.position = new Vector3(
            this._tempGhostBuild.position.x,
            0,
            this._tempGhostBuild.position.z
        );
        build.rotation = this._tempGhostBuild.rotation;
        
        List<BuildingCost> costList = (List<BuildingCost>)this._currentGhostBuild.Costs;
        Dictionary<ResourceType, int> costDic = new Dictionary<ResourceType, int>();
            
        foreach (BuildingCost buildingCost in costList)
        {
            costDic[buildingCost.ResourceType] = buildingCost.Cost;
        }
        
        ResourceManager.Instance.TakeResource(costDic);
        this._buildingHashList.Add(build);
        this.DeativateGhostBuilding();
    }
}
