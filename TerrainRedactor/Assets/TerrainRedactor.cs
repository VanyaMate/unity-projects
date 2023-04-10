using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRedactor : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;

    private int _heightmapWidth;
    private int _heightmapHeight;

    private void Awake()
    {
        if (this._terrain != null)
        {
            this._heightmapWidth = this._terrain.terrainData.heightmapResolution;
            this._heightmapHeight = this._terrain.terrainData.heightmapResolution;

            this.SetRandomHeights();
        }
    }

    private void SetRandomHeights ()
    {
        float[,] heights = this._terrain.terrainData.GetHeights(0, 0, this._heightmapWidth, this._heightmapHeight);
    
        for (int x = 0; x < this._heightmapWidth; x++)
        {
            for (int z = 0; z < this._heightmapHeight; z++)
            {
                heights[x, z] = Random.Range(0, .0002f);
            }
        }

        this._terrain.terrainData.SetHeights(0, 0, heights);
    }
}
