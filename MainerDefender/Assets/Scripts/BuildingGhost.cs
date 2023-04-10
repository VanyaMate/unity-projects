using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingGhost : MonoBehaviour
{
    private GameObject _ghostObject;
    private Camera _mainCamera;
    private Vector3 _lastMousePoint;

    private void Awake()
    {
        this._mainCamera = Camera.main;
        this._lastMousePoint = Vector3.zero;
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManager_OnActiveBuildingTypeChange;
    }

    private void BuildingManager_OnActiveBuildingTypeChange(object sender, System.EventArgs e)
    {
        BuildingTypeSO buildingType = BuildingManager.Instance.GetActiveBuildingType();

        this.Hide();

        if (buildingType != null && EventSystem.current.IsPointerOverGameObject())
        {
            this.Show(buildingType.Pf.gameObject);
        }
    }

    private void Update()
    {
        if (this._ghostObject != null)
        {
            this._ghostObject.transform.position = this._lastMousePoint = UtilsClass.GetMouseWorldPosition(this._mainCamera, this._lastMousePoint);
        }
    }

    private void Show (GameObject pf)
    {
        this._ghostObject = Instantiate(pf, transform);
        this._ghostObject.layer = 2;

        this._ghostObject.GetComponent<ResourceGenerator>().enabled = false;
    
        Transform ghostModel = this._ghostObject.transform.Find("Model");

        ghostModel.GetComponent<SphereCollider>().enabled = false;

        Material ghostModelMaterial = ghostModel.GetComponent<Renderer>().material;
        Color ghostModelColor = ghostModelMaterial.color;
        ghostModelMaterial.color = new Color(ghostModelColor.r, ghostModelColor.g, ghostModelColor.b, .5f);
    }

    private void Hide ()
    {
        Destroy(this._ghostObject);
        this._ghostObject = null;
    }
}
