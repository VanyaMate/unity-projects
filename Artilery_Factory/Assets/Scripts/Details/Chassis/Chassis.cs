using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chassis : MonoBehaviour
{
    [SerializeField] private ChassisSO _chassisType;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        this._navMeshAgent.acceleration = this._chassisType.AccelerationSpeed;
        this._navMeshAgent.speed = this._chassisType.MaxSpeed;
        this._navMeshAgent.angularSpeed = this._chassisType.RotationSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();

            this._navMeshAgent.SetDestination(mouseWorldPosition);
        }
    }
}
