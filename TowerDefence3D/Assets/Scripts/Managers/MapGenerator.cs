using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
class ResourceWeigth
{
    public Transform ResourcePrefab;
    public float Weight;
}

[Serializable]
class ResourcePlaceSettings
{
    public ResourceType ResourceType;
    public int AmountOnMap;
    public List<ResourceWeigth> Weigths = new List<ResourceWeigth>();

    public Dictionary<int, Transform> WeightDictionary = new Dictionary<int, Transform>();
}

[Serializable]
class GeneratorSettings
{
    public int PaddingX;
    public int PaddingY;

    public int RandomRangeStart;
    public int RandomRangeEnd;

    public int RandomRangeNearCenter;

    public int ExcludeCenterRange;
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Terrain _map;
    [SerializeField] private Transform _resourceMap;
    [SerializeField] private List<ResourcePlaceSettings> _placeSettings = new List<ResourcePlaceSettings>();

    [SerializeField] private GeneratorSettings _settings = new GeneratorSettings();
    
    private void Start()
    {
        Vector3 terrainSize = this._map.terrainData.size;
        float terrainWidth = terrainSize.x - this._settings.PaddingX;
        float terrainLength = terrainSize.z - this._settings.PaddingY;

        foreach (ResourcePlaceSettings resourcePlaceSettings in this._placeSettings)
        {
            int counter = 0;
            foreach (ResourceWeigth resourceWeigth in resourcePlaceSettings.Weigths)
            {
                for (int i = 1; i <= resourceWeigth.Weight; i++)
                {
                    resourcePlaceSettings.WeightDictionary[i + counter] = resourceWeigth.ResourcePrefab;
                }
            }

            for (int i = 0; i < resourcePlaceSettings.AmountOnMap; i++)
            {
                int amountResourceNodes = Random.Range(this._settings.RandomRangeStart, this._settings.RandomRangeEnd);
                Vector2 randomPosition = this.GetRandomPosition(terrainWidth, terrainLength);
                int centerXOfResourceNodes = (int)randomPosition.x;
                int centerZOfResourceNodes = (int)randomPosition.y;
                
                for (int y = 0; y < amountResourceNodes; y++)
                {
                    Transform resourceNodePrefab =
                        resourcePlaceSettings.WeightDictionary[
                            Random.Range(1, resourcePlaceSettings.WeightDictionary.Count)
                        ];
                    Transform resourceNode = Instantiate(resourceNodePrefab, this._resourceMap);

                    resourceNode.position = new Vector3(
                        centerXOfResourceNodes + Random.Range(-this._settings.RandomRangeNearCenter, this._settings.RandomRangeNearCenter),
                        0,
                        centerZOfResourceNodes + Random.Range(-this._settings.RandomRangeNearCenter, this._settings.RandomRangeNearCenter)
                    );
                    resourceNode.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                }
            }
        }
    }

    public Vector2 GetRandomPosition(float terrainWidth, float terrainLength)
    {
        Vector2 position = new Vector2(
            Random.Range(0 - (int)(terrainWidth / 2), (int)(terrainWidth / 2)),
            Random.Range(0 - (int)(terrainLength / 2), (int)(terrainLength / 2))    
        );

        if (
            (position.x > this._settings.ExcludeCenterRange / 2 ||
            position.x < 0 - this._settings.ExcludeCenterRange / 2) &&            
            (position.y > this._settings.ExcludeCenterRange / 2 ||
            position.y < 0 - this._settings.ExcludeCenterRange / 2)
        )
        {
            return position;
        }
        else
        {
            return this.GetRandomPosition(terrainWidth, terrainLength);
        }
    }
}
