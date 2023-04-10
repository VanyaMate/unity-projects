using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Transform _btnTemplate;
    [SerializeField] private Sprite _arrowSprite;

    private BuildingTypeListSO _buildingTypes;
    private Dictionary<BuildingTypeSO, Transform> _buildingTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
    private Transform _arrowBtn;

    private void Awake()
    {
        this._buildingTypes = Resources.Load<BuildingTypeListSO>("buildingTypeListSO_default");

        float index = 1;
        float offsetAmount = 115f;

        this._arrowBtn = Instantiate(this._btnTemplate, transform);

        this._arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        this._arrowBtn.Find("icon").GetComponent<Image>().sprite = this._arrowSprite;
        this._arrowBtn.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                this.SetBuildingType(null);
                this.UpdateActiveBuildingTypeButton();
            }
        );

        foreach (BuildingTypeSO buildingType in this._buildingTypes.List)
        {
            Transform selectBtn = Instantiate(this._btnTemplate, transform);

            selectBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * offsetAmount, 0);
            selectBtn.Find("icon").GetComponent<Image>().sprite = buildingType.Icon;
            selectBtn.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    this.SetBuildingType(buildingType);
                    this.UpdateActiveBuildingTypeButton();
                }
            );

            this._buildingTransformDictionary[buildingType] = selectBtn;

            index += 1;
        }
    }

    private void Start()
    {
        this.UpdateActiveBuildingTypeButton();
    }

    private void SetBuildingType (BuildingTypeSO buildingType)
    {
        BuildingManager.Instance.SetActiveBuildingType(buildingType);
    }

    private void UpdateActiveBuildingTypeButton()
    {
        foreach (BuildingTypeSO buildingType in this._buildingTransformDictionary.Keys)
        {
            Transform selectBtn = this._buildingTransformDictionary[buildingType];

            selectBtn.Find("select").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if (activeBuildingType != null)
        {
            this._buildingTransformDictionary[activeBuildingType].Find("select").gameObject.SetActive(true);
            this._arrowBtn.Find("select").gameObject.SetActive(false);
        }
        else
        {
            this._arrowBtn.Find("select").gameObject.SetActive(true);
        }
    }
}
