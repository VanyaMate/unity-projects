using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_groundSettings_", menuName = "game/ground/create type", order = 51)]
public class GroundSettings : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Transform _prefab;
    [SerializeField] private float _speedMultiply;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public Transform Prefab => _prefab;
    public float SpeedMultiply => _speedMultiply;
}
