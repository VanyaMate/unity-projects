using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public static ResourceUI Instance;

    [SerializeField] private Transform _resourceNodePrefab;
 
    private Dictionary<ResourceType, ResourceNodeUI> _resourceNodeUis = new Dictionary<ResourceType, ResourceNodeUI>();

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateData(ResourceType resourceType, int amount)
    {
        if (amount == 0)
        {
            Destroy(this._resourceNodeUis[resourceType].gameObject);
            this._resourceNodeUis.Remove(resourceType);

            int counter = 1;
            foreach (ResourceType _resourceType in this._resourceNodeUis.Keys)
            {
                this._resourceNodeUis[_resourceType].transform.localPosition = new Vector3(counter * -200 + 100, -50, 0);
            }
        }
        else
        {
            ResourceNodeUI resourceNodeUI = null;
            bool createdNow = !this._resourceNodeUis.TryGetValue(resourceType, out resourceNodeUI);

            if (createdNow)
            {
                Instantiate(this._resourceNodePrefab, transform).TryGetComponent<ResourceNodeUI>(out resourceNodeUI);
                this._resourceNodeUis[resourceType] = resourceNodeUI;
                
                resourceNodeUI.transform.localPosition = new Vector3(this._resourceNodeUis.Count * -200 + 100, -50, 0);
                resourceNodeUI.SetIcon(resourceType.Icon);
            }

            resourceNodeUI.UpdateAmount(amount);
        }
    }
}
