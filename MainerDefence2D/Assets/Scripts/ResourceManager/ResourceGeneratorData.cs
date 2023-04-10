using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceGeneratorData
{
    [SerializeField] private float _timerMax;
    [SerializeField] private ResourceTypeSO _resourceType;
    [SerializeField] private float _resourceDetectionRadius;
    [SerializeField] private int _maxResourceAmount;

    public float Timer { get => _timerMax; set => _timerMax = value; }
    public ResourceTypeSO Resource => _resourceType;
    public float ResourseDetectionRadius => _resourceDetectionRadius;
    public int MaxResourceAmount => _maxResourceAmount;
}
