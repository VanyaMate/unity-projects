using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int>();

    public Dictionary<ResourceType, int> ResourceDic => _resources;
    public UnityAction OnAddResource;
    public UnityAction OnTakeResource;

    private void Awake()
    {
        Instance = this;
    }

    public void AddResource(Dictionary<ResourceType, int> resources)
    {
        foreach (ResourceType resourceType in resources.Keys)
        {
            int currentResourceAmount = 0;

            this._resources.TryGetValue(resourceType, out currentResourceAmount);
            this._resources[resourceType] = currentResourceAmount + resources[resourceType];
            
            ResourceUI.Instance.UpdateData(resourceType, this._resources[resourceType]);
        }
        
        this.OnAddResource?.Invoke();
    }

    public bool TakeResource(Dictionary<ResourceType, int> resources, bool check = false)
    {
        bool allResourcesAvailable = true;
        
        foreach (ResourceType resource in resources.Keys)
        {
            if (!(this._resources.ContainsKey(resource) && this._resources[resource] >= resources[resource]))
            {
                allResourcesAvailable = false;
                break;
            }
        }

        if (!check && allResourcesAvailable)
        {
            foreach (ResourceType resource in resources.Keys)
            {
                this._resources[resource] -= resources[resource];
                ResourceUI.Instance.UpdateData(resource, this._resources[resource]);
            }
            
            this.OnTakeResource?.Invoke();
        }

        return allResourcesAvailable;
    }
}
