using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBase : MonoBehaviour
{
    public static MapBase Instance;
    
    [SerializeField] private Transform _buildings;
    [SerializeField] private Transform _resources;

    public Transform Buildings => _buildings;
    public Transform Resources => _resources;

    private void Awake()
    {
        Instance = this;
    }
}
