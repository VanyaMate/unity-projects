using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public event EventHandler OnResourceAmountChange;

    private Dictionary<ResourceTypeSO, int> _resources = new Dictionary<ResourceTypeSO, int>();

    private void Awake()
    {
        Instance = this;

        ResourceTypeListSO resourceList = Resources.Load<ResourceTypeListSO>("SO_resourceTypeList_default");

        foreach (ResourceTypeSO resourceType in resourceList.List)
        {
            this._resources[resourceType] = 0;
        }
    }

    private void _TestLogResources()
    {
        foreach (ResourceTypeSO resourceType in _resources.Keys)
        {
            Debug.Log($"{ resourceType.Name }: { this._resources[resourceType] }");
        }
    }

    public int GetResourceAmount (ResourceTypeSO resourceType)
    {
        return this._resources[resourceType];
    }

    public void AddResource (ResourceTypeSO resourceType, int amount)
    {
        this._resources[resourceType] += amount;
        this.OnResourceAmountChange?.Invoke(this, EventArgs.Empty);
    }
}
