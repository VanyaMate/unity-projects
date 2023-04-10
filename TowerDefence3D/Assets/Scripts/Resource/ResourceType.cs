using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_resourceType_", menuName = "game/resource/type/create", order = 51)]
public class ResourceType : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;

    public string Name => _name;
    public Sprite Icon => _icon;
}
