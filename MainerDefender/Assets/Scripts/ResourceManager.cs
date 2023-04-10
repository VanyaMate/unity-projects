using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;

    public event EventHandler OnResourceAmountChanged;

    private void Awake()
    {
        Instance = this;

        this._resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>("resourseTypeListSO_default");
    
        foreach (ResourceTypeSO resourceType in resourceTypeList.List)
        {
            this._resourceAmountDictionary[resourceType] = 0;
        }

        this.TestLogResourceAmountDictionary();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>("resourseTypeListSO_default");
            AddResource(resourceTypeList.List[0], 2);
            TestLogResourceAmountDictionary();
        }
    }

    private void TestLogResourceAmountDictionary ()
    {
        foreach (ResourceTypeSO resourceType in this._resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.Name + ": " + this._resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        this._resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        this.TestLogResourceAmountDictionary();
    }

    public int GetResourceAmount(ResourceTypeSO resourseType)
    {
        return this._resourceAmountDictionary[resourseType]; 
    }
}
