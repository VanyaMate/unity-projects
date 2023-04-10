using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    private Dictionary<Vector2, MapPoint> _mapData = new Dictionary<Vector2, MapPoint>();
    private Vector2 _mapSize;

    public MapSystem(Vector2 mapSize)
    {
        this._mapSize = mapSize;
    }

    private void Awake()
    {
        
    }
}
