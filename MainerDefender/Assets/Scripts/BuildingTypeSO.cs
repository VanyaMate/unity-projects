using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "buildingTypeSO_", menuName = "Game/Building/SO/Create", order = 51)]
public class BuildingTypeSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Transform _pf;
    [SerializeField] private ResourceGeneratorData _resourceGeneratorData; 

    public string Name => this._name;
    public Transform Pf => this._pf;
    public ResourceGeneratorData ResourceGeneratorData => _resourceGeneratorData;
    public Sprite Icon => _icon;
}
