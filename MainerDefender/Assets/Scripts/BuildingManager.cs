using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Transform _debugMark;

    public static BuildingManager Instance { get; private set; }
    public event EventHandler OnActiveBuildingTypeChange;

    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _buildingType;
    private Camera _mainCamera;
    private Vector3 _lastMousePoint;

    public BuildingTypeSO BuildingType { 
        get => this._buildingType; 
        private set => this._buildingType = value;
    }

    private void Awake()
    {
        Instance = this;
        this._buildingTypeList = Resources.Load<BuildingTypeListSO>("buildingTypeListSO_default");
        this._buildingType = null;
    }

    private void Start()
    {
        this._mainCamera = Camera.main;
        this._lastMousePoint = Vector3.zero;
    }

    private void Update()
    {
        Vector3 mousePosition = this._lastMousePoint = UtilsClass.GetMouseWorldPosition(this._mainCamera, this._lastMousePoint);
        
        if (mousePosition != null)
        {
            this._debugMark.position = mousePosition;

            if (
                Input.GetMouseButtonDown(0) && 
                !EventSystem.current.IsPointerOverGameObject() &&
                this._buildingType != null
            )
            {
                Instantiate(this._buildingType.Pf, mousePosition, Quaternion.identity);
            }
        }
    }

    public void SetActiveBuildingType (BuildingTypeSO buildingType)
    {
        this._buildingType = buildingType;
        this.OnActiveBuildingTypeChange?.Invoke(this, EventArgs.Empty);
    }

    public BuildingTypeSO GetActiveBuildingType ()
    {
        return this._buildingType;
    }
}
