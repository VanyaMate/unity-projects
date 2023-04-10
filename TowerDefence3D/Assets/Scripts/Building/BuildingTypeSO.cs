using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class BuildingCost
{
    public ResourceType ResourceType;
    public int Cost;
}

[CreateAssetMenu(fileName = "so_buildingType_", menuName = "game/building/create type", order = 51)]
public class BuildingTypeSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Transform _prefabModel;
    [SerializeField] private Transform _prefab;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ResourceType _mainResource;
    [SerializeField] private List<BuildingCost> _costs = new List<BuildingCost>();

    public string Name => _name;
    public Transform PrefabModel => _prefabModel;
    public Transform Prefab => _prefab;
    public Sprite Sprite => _sprite;
    public ResourceType MainResource => _mainResource;
    public object Costs => _costs as List<BuildingCost>;
}
