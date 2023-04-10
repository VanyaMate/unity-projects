using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private Transform _buildingTemplate;
    [SerializeField] private Sprite _arrowButtonSprite;

    private Dictionary<BuildingTypeSO, BuildingDataUI> _buildingButtons = new Dictionary<BuildingTypeSO, BuildingDataUI>();
    private BuildingTypeListSO _buildingList;
    private BuildingDataUI _selectedButton;
    private BuildingDataUI _arrowButton;

    private void Awake()
    {
        this._buildingList = Resources.Load<BuildingTypeListSO>("SO_buildingTypeList_default");
        this._arrowButton = new BuildingDataUI();

        Transform arrowButton = this._arrowButton.UI = Instantiate(this._buildingTemplate, transform);

        this._arrowButton.Icon = arrowButton.Find("icon").GetComponent<Image>();
        this._arrowButton.SelectMark = arrowButton.Find("selectMark").GetComponent<Image>();
        this._arrowButton.Button = arrowButton.Find("background").GetComponent<Button>();
        this._arrowButton.Icon.sprite = this._arrowButtonSprite;

        arrowButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(15, 15);
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingChanged += _BuildingManager_OnActiveBuildingChanged;
        this._arrowButton.Button.onClick.AddListener(() => BuildingManager.Instance.SetActiveBuildingType(null));

        int index = 1;
        int offsetAmount = 160;

        foreach (BuildingTypeSO buildingType in this._buildingList.List)
        {
            BuildingDataUI buildingButtonData = this._buildingButtons[buildingType] = new BuildingDataUI();
            Transform buildingButton = buildingButtonData.UI = Instantiate(this._buildingTemplate, transform);

            buildingButtonData.Icon = buildingButton.Find("icon").GetComponent<Image>();
            buildingButtonData.SelectMark = buildingButton.Find("selectMark").GetComponent<Image>();
            buildingButtonData.Button = buildingButton.Find("background").GetComponent<Button>();

            buildingButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index + 15, 15);
            buildingButtonData.Icon.sprite = buildingType.Icon;
            buildingButtonData.Button.onClick.AddListener(() => BuildingManager.Instance.SetActiveBuildingType(buildingType));

            index += 1;
        }

        this._UpdateSelectedButton();
    }

    private void _BuildingManager_OnActiveBuildingChanged(object sender, System.EventArgs e)
    {
        this._UpdateSelectedButton();
    }

    private void _UpdateSelectedButton()
    {
        if (this._selectedButton != null)
        {
            this._selectedButton.SelectMark.enabled = false;
        }

        this._selectedButton = BuildingManager.Instance.ActiveBuildingType != null 
            ? this._buildingButtons[BuildingManager.Instance.ActiveBuildingType] 
            : this._arrowButton;

        this._selectedButton.SelectMark.enabled = true;
        this._selectedButton.Button.Select();
    }
}
