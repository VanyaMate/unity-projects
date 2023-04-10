using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_buildingType_", menuName = "Game/Building/Create Type", order = 51)]
public class BuildingTypeSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Transform _prefab;
    [SerializeField] private ResourceGeneratorData _resourceGenerator;

    public string Name => _name;
    public Sprite Icon => _icon;
    public Transform Prefab => _prefab;
    public ResourceGeneratorData Generator => _resourceGenerator;
}
