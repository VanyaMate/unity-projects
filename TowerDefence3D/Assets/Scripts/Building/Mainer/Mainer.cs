using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainer : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _mainCounter;
    [SerializeField] private float _mainRate;
    [SerializeField] private float _mainRange;

    private IEnumerator _mainTimer;
    private Collider[] _allResourceNodes;
    private List<ResourceNode> _resourceNodes = new List<ResourceNode>();
    
    private void Start()
    {
        StartCoroutine(this._mainTimer = this.DoMain());
    }

    private IEnumerator DoMain()
    {
        while (true)
        {
            yield return new WaitForSeconds(this._mainRate);
            
            ResourceNode nearestResourceNode = this.GetNearestResourceNode();

            if (nearestResourceNode)
            {
                ResourceManager.Instance.AddResource(nearestResourceNode.Take(this._mainCounter));
            }
            else
            {
                StopCoroutine(this._mainTimer);
                Destroy(this);
            } 
        }
    }

    private ResourceNode GetNearestResourceNode()
    {
        this._resourceNodes = new List<ResourceNode>();
        this._allResourceNodes = Physics.OverlapSphere(
            transform.position, 
            this._mainRange, 
            LayerMask.GetMask("ResourceNode")
        );

        foreach (Collider resource in this._allResourceNodes)
        {
            ResourceNode resourceNode = resource.GetComponent<ResourceNode>();

            if (resourceNode && resourceNode.ResourceType == this._resourceType)
            {
                this._resourceNodes.Add(resourceNode);
            }
        }
        
        float nearestNodeDistance = this._mainRange + 1;
        ResourceNode nearestNode = null;
        foreach (ResourceNode resource in this._resourceNodes)
        {
            float distance = Vector3.Distance(transform.position, resource.transform.position);
            if (distance < nearestNodeDistance)
            {
                nearestNodeDistance = distance;
                nearestNode = resource;
            }
        }
        
        return nearestNode;
    }
}
