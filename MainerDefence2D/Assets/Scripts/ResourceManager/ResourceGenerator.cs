using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData _resourceGenerator;
    private BuildingTypeHolder _buildingHolder;
    private float _timer;
    private float _maxTimer;

    private void Awake()
    {
        this._buildingHolder = GetComponent<BuildingTypeHolder>();
        this._resourceGenerator = this._buildingHolder.Type.Generator;
    }

    private void Start()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, this._resourceGenerator.ResourseDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();

            if (resourceNode.Type == this._resourceGenerator.Resource)
            {
                nearbyResourceAmount += 1;
            }
        }


        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, this._resourceGenerator.MaxResourceAmount);

        Debug.Log("ResourceNearby: " + nearbyResourceAmount);

        if (nearbyResourceAmount == 0)
        {
            this.enabled = false;
        }
        else
        {
            this._maxTimer = this._timer = this._resourceGenerator.Timer * ((float)this._resourceGenerator.MaxResourceAmount / (float)nearbyResourceAmount);
            Debug.Log(this._timer);
        }
    }

    private void Update()
    {
        this._timer -= Time.deltaTime;

        if (this._timer < 0)
        {
            ResourceManager.Instance.AddResource(this._buildingHolder.Type.Generator.Resource, 1);
            this._timer = this._maxTimer;
        }
    }
}
