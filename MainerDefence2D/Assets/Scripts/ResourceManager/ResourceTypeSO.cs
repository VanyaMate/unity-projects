using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_resourceType_", menuName = "Game/Resource/Create Type", order = 51)]
public class ResourceTypeSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;

    public string Name => _name;
    public Sprite Icon => _icon;
}
