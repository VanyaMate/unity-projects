using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_gun_", menuName = "game/gun/create", order = 51)]
public class GunSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _startSpeed;
    [SerializeField] private BulletSO _bulletType;
    [SerializeField] private bool _automat;
    [SerializeField] private int _fireRate;
    [SerializeField] private AudioClip _shootSound;

    public string Name => _name;
    public float StartSpeed => _startSpeed;
    public BulletSO BulletType => _bulletType;
    public bool Automat => _automat;
    public int FireRate => _fireRate;
    public AudioClip ShootSound => _shootSound;
}
