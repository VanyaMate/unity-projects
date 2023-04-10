using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_bullet_", menuName = "game/gun/bullet/create", order = 51)]
public class BulletSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _damage;
    [SerializeField] private Transform _prefab;
    [SerializeField] private Transform _hitEffect;

    public string Name => _name;
    public float Damage => _damage;
    public Transform Prefab => _prefab;
    public Transform HitEffect => _hitEffect;
}
