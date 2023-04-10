using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Buildings
{
    [Serializable]
    public class BuildingGridSystem
    {
        [SerializeField] private Vector3 _gridSizes;
        [SerializeField] private bool _showGridPoint;

        public Vector3 GetGridPositionBy(Vector3 position)
        {
            return new Vector3(
                Mathf.Floor(position.x * this._gridSizes.x) / this._gridSizes.x,    
                Mathf.Floor(position.y * this._gridSizes.y) / this._gridSizes.y,    
                Mathf.Floor(position.z * this._gridSizes.z) / this._gridSizes.z
            );
        }
    }
}