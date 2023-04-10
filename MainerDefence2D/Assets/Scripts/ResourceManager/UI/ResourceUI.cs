using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private Transform _resourceTemplate;

    private ResourceTypeListSO _resourceTypes;
    private Dictionary<ResourceTypeSO, ResourceDataUI> _resources = new Dictionary<ResourceTypeSO, ResourceDataUI>();

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceAmountChange -= this._ResourceManager_OnResourceAmountChange;
    }

    private void _ResourceManager_OnResourceAmountChange(object sender, System.EventArgs e)
    {
        this._UpdateResourceAmount();
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChange += this._ResourceManager_OnResourceAmountChange;
        this._resourceTypes = Resources.Load<ResourceTypeListSO>("SO_resourceTypeList_default");

        int index = 0;
        int offsetAmount = 200;

        foreach (ResourceTypeSO resourceType in this._resourceTypes.List)
        {
            Transform resourceUI = Instantiate(this._resourceTemplate, transform);
            ResourceDataUI resourceData = this._resources[resourceType] = new ResourceDataUI();

            resourceUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-index * offsetAmount, 0);

            resourceData.UI = resourceUI;
            resourceData.IconComponent = resourceUI.Find("icon").GetComponent<Image>();
            resourceData.AmountComponent = resourceUI.Find("amount").GetComponent<TMP_Text>();

            resourceData.IconComponent.sprite = resourceType.Icon;
            resourceData.AmountComponent.text = ResourceManager.Instance.GetResourceAmount(resourceType).ToString();

            index += 1;
        }
    }

    private void _UpdateResourceAmount ()
    {
        foreach (ResourceTypeSO resourceType in this._resourceTypes.List)
        {
            this._resources[resourceType].AmountComponent.text = ResourceManager.Instance.GetResourceAmount(resourceType).ToString();
        }
    }
}
    