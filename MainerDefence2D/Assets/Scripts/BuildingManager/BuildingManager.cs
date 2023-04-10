using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
class BuildingSettings
{
    public Camera MainCamera;
    public Transform DebugMark;
}

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }
    public event EventHandler OnActiveBuildingChanged;

    [SerializeField] private BuildingSettings _settings = new BuildingSettings();
    [SerializeField] private BuildingTypeSO _activeBuildingType;

    private BuildingTypeListSO _buildingTypeList;

    public BuildingTypeSO ActiveBuildingType => this._activeBuildingType;

    private void Awake()
    {
        Instance = this;

        this._settings.MainCamera = Camera.main;
        this._buildingTypeList = Resources.Load<BuildingTypeListSO>("SO_buildingTypeList_default");
    }

    private void Update()
    {
        // Дебаг
        if (this._settings.DebugMark != null)
        {
            this._settings.DebugMark.position = UtilsClass.GetMouseWorldPosition();
        }

        // Уставновить активную постройку
        if (
            this._activeBuildingType != null && 
            Input.GetMouseButtonDown(0) && 
            !EventSystem.current.IsPointerOverGameObject()
        )
        {
            Transform building = Instantiate(
                this._activeBuildingType.Prefab,
                UtilsClass.GetMouseWorldPosition(), 
                Quaternion.identity
            );
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        this._activeBuildingType = buildingType;
        this.OnActiveBuildingChanged?.Invoke(this, EventArgs.Empty);
    }
}
