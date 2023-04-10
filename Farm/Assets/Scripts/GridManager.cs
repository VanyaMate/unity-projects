using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform _map;
    [SerializeField] private GroundSettings _defaultGroundSettings;
    [SerializeField] private GridPoint _gridPoint;

    private Dictionary<Vector2, GridPoint> _mapData = new Dictionary<Vector2, GridPoint>();

    private void Awake()
    {
        float mapWidth = this._map.GetComponent<Terrain>().terrainData.size.x;
        float mapLength = this._map.GetComponent<Terrain>().terrainData.size.z;
        int mapStartX = (int)mapWidth / 2;
        int mapStartZ = (int)mapLength / 2;

        Debug.Log(mapStartX);

        for (int x = -mapStartX; x < mapStartX; x++)
        {
            for (int z = -mapStartZ; z < mapStartZ; z++)
            {
                Vector2 position = new Vector2(x, z);
                GridPoint point = Instantiate(this._gridPoint, this._map.transform);

                this._mapData.Add(position, point);

                point.SetGround(this._defaultGroundSettings, position);
            }
        }
    }
}
