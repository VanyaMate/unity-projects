using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Gun_", menuName = "Game/Gun/Create gun", order = 51)]
public class GunSO : ScriptableObject
{
    [SerializeField] private float _fireRate;
    [SerializeField] private float _maxFireRange;
    [SerializeField] private float _minFireRange;
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private float _maxTrunkSpeed;
    [SerializeField] private BulletSO _ammoType;
    [SerializeField] private float _ammoStartSpeed;
    [SerializeField] private bool _mounted;
    [SerializeField] private GameObject _gunExplosionEffect;

    public float FireRate => _fireRate;
    public float MaxFireRange => _maxFireRange;
    public float MinFireRange => _minFireRange;
    public float MaxRotationSpeed => _maxRotationSpeed;
    public float MaxTrunkSpeed => _maxTrunkSpeed;
    public BulletSO AmmoType => _ammoType;
    public float AmmoStartSpeed => _ammoStartSpeed;
    public bool Mounted => _mounted;
    public GameObject GunExplostionEffect => _gunExplosionEffect;
}
