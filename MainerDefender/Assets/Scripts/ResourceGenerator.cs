using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private ResourceManager _resourceManager;

    private BuildingTypeSO _buildingType;
    private float _timer;
    private float _timerMax;

    private void Awake()
    {
        this._buildingType = GetComponent<BuildingTypeHolder>().Type;
        this._timerMax = this._buildingType.ResourceGeneratorData.TimerMax;
    }

    private void Update()
    {
        this._timer -= Time.deltaTime;

        if (this._timer <= 0f)
        {
            this._timer += this._timerMax;

            ResourceManager.Instance.AddResource(this._buildingType.ResourceGeneratorData.ResourceType, 1);
        }
    }
}
