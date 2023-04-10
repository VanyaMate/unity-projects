using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Bullet_", menuName = "Game/Bullet/Create bullet", order = 51)]
public class BulletSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _penitration;
    [SerializeField] private float _damage;
    [SerializeField] private Transform _prefab;
    [SerializeField] private GameObject _explosionEffect;

    public string Name => _name;
    public float Penitration => _penitration;
    public float Damage => _damage;
    public Transform Prefab => _prefab;
    public GameObject ExplosionEffect => _explosionEffect;
}
