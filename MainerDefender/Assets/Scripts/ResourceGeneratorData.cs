using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceGeneratorData
{
    [SerializeField] private float _timerMax;
    [SerializeField] private ResourceTypeSO _resourceType;

    public float TimerMax { get => _timerMax; }
    public ResourceTypeSO ResourceType { get => _resourceType; }
}
