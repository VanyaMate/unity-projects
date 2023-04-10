using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public static GameUtils Instance;
    
    [SerializeField] private Camera _mainCamera;
    
    private void Awake()
    {
        Instance = this;
        this._mainCamera = Camera.main;
    }

    public RaycastHit GetMouseWorldPositionHit()
    {
        RaycastHit hit;
        Ray ray = this._mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit;
        }
        else
        {
            return default;
        }
    }
}
