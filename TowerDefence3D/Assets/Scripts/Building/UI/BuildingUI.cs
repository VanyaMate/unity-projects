using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private BuildingTypeListSO _buildingTypeListSo;
    [SerializeField] private Transform _buildingTypeUIPrefab;

    private Dictionary<Transform, BuildingTypeSO> _buildings = new Dictionary<Transform, BuildingTypeSO>();

    private void Start()
    {
        int startPosition = 55;
        int anchorPosiiton = 105;

        ResourceManager.Instance.OnAddResource += this.CheckAvailableBuildings;
        ResourceManager.Instance.OnTakeResource += this.CheckAvailableBuildings;
        
        foreach (BuildingTypeSO buildingTypeSo in this._buildingTypeListSo.List)
        {
            Transform buildingUI = Instantiate(this._buildingTypeUIPrefab, transform);
            BuildingTypeUI buildingTypeUI = buildingUI.GetComponent<BuildingTypeUI>();
            Button buildingButton = buildingUI.GetComponent<Button>();

            buildingUI.localPosition = new Vector3(startPosition, 55, 0);
            
            buildingTypeUI.Icon.sprite = buildingTypeSo.Sprite;
            buildingTypeUI.BuildingTypeSo = buildingTypeSo;
            
            buildingButton.onClick.AddListener(() =>
                {
                    if (buildingUI.GetComponent<Image>().color.a == .8f)
                    {
                        BuildingManager.Instance.ActivateGhostBuilding(buildingTypeSo);
                    }
                }
            );

            this._buildings[buildingUI] = buildingTypeSo;
            
            startPosition += anchorPosiiton;
        }

        this.CheckAvailableBuildings();
    }

    private void CheckAvailableBuildings()
    {
        foreach (Transform buildingUI in this._buildings.Keys)
        {
            List<BuildingCost> costList = (List<BuildingCost>)this._buildings[buildingUI].Costs;
            Dictionary<ResourceType, int> costDic = new Dictionary<ResourceType, int>();
            
            foreach (BuildingCost buildingCost in costList)
            {
                costDic[buildingCost.ResourceType] = buildingCost.Cost;
            }

            if (ResourceManager.Instance.TakeResource(costDic, true))
            {
                buildingUI.GetComponent<Image>().color = new Color(255, 255, 255, .8f);
            }
            else
            {
                buildingUI.GetComponent<Image>().color = new Color(255, 255, 255, .2f);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BuildingManager.Instance.DeativateGhostBuilding();
        }
    }
}
