using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BotSide_", menuName = "Game/Bot/Side/Create side", order = 51)]
public class BotSideSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Material _color;

    public string Name => _name;
    public Material Color => _color;
}
