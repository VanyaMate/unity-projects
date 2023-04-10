using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _amount;

    public ResourceType ResourceType => _resourceType;

    public void ClickAction(int amount)
    {
        this.Take(amount);
    }

    public Dictionary<ResourceType, int> Take(int amount)
    {
        int gettedAmount = amount;

        if (this._amount - amount > 0)
        {
            this._amount -= amount;
        }
        else
        {
            gettedAmount = this._amount;
            Destroy(gameObject);
        }

        return new Dictionary<ResourceType, int>()
        {
            {
                this._resourceType, gettedAmount
            }
        };
    }
}
