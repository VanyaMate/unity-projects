using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Chassis_", menuName = "Game/Chassis/Create chassis", order = 51)]
public class ChassisSO : ScriptableObject
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelerationSpeed;
    [SerializeField] private float _rotationSpeed;

    public float MaxSpeed => _maxSpeed;
    public float AccelerationSpeed => _accelerationSpeed;
    public float RotationSpeed => _rotationSpeed;
}
