using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG.Managers
{
    public class MapTileRedactor : MonoBehaviour
    {
        public static MapTileRedactor Instance;

        [SerializeField] private Terrain _map;

        private void Awake()
        {
            Instance = this;

            this._FillMapGrass();
        }

        public void DeleteGrass (Vector2 point)
        {
            int[,] map = this._map.terrainData.GetDetailLayer(0, 0, (int)point.x + 10, (int)point.y + 10, 0);
        }

        private void _FillMapGrass ()
        {
            int[,] newMap = new int[this._map.terrainData.detailWidth, this._map.terrainData.detailHeight];

            for (int i = 0; i < this._map.terrainData.detailWidth; i += 1)
            {
                for (int j = 0; j < this._map.terrainData.detailHeight; i += 1)
                {
                    newMap[i, j] = 1;
                }
            }

            this._map.terrainData.SetDetailLayer(0, 0, 0, newMap);
        }
    }
}
