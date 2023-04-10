using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private Transform _resourceTemplate;

    private ResourceTypeListSO _resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> _resourceTransforms = new Dictionary<ResourceTypeSO, Transform>();

    private void Awake()
    {
        this._resourceTypeList = Resources.Load<ResourceTypeListSO>("resourseTypeListSO_default");

        int index = 0;
        float offsetAmount = -160f;
        foreach (ResourceTypeSO resourceType in this._resourceTypeList.List)
        {
            this._resourceTransforms[resourceType] = Instantiate(this._resourceTemplate, transform);
            this._resourceTransforms[resourceType].GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            this._resourceTransforms[resourceType].Find("icon").GetComponent<Image>().sprite = resourceType.Icon;

            index += 1;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += UpdateResourceEvent;
        this.UpdateResourceAmount();
    }

    private void UpdateResourceEvent(object sender, System.EventArgs e)
    {
        this.UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in this._resourceTypeList.List)
        {
            string resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType).ToString();
            this._resourceTransforms[resourceType].Find("amount").GetComponent<TMP_Text>().text = resourceAmount;
        }
    }
}
